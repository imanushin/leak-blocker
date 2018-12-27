using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.SystemTools;

namespace LeakBlocker.Libraries.Common.Network
{
    internal sealed class BaseNetworkHost : Disposable, IBaseNetworkHost
    {
        private readonly ReadOnlySet<TcpListener> listeners;

        private readonly ConcurrentDictionary<string, BaseServer> registeredServers = new ConcurrentDictionary<string, BaseServer>();

        private readonly  TimeSpan maxOperationTimeout;

        private readonly ReadOnlySet<IWaitHandle> listenerThreads;

        private readonly IThreadPool threadPool = SharedObjects.CreateThreadPool(64);

        public BaseNetworkHost(int tcpPort, TimeSpan operationTimeout)
        {
            maxOperationTimeout = operationTimeout;
            IEnumerable<IPAddress> addresses = NetworkInterface.GetAllNetworkInterfaces().Select(networkInterface => 
                networkInterface.GetIPProperties()).SelectMany(properties => properties.UnicastAddresses).Select(address => address.Address);

            string addressString = string.Join(", ", addresses.Select(addr => addr.ToString()));

            Log.Add("Network interface addresses: {0}".Combine(addressString));

            listeners = addresses.Select(address => new TcpListener(address, tcpPort)).ToReadOnlySet();

            IEnumerable<TcpListener> startedListeners = listeners.Select(delegate(TcpListener listener)
            {
                try
                {
                    listener.Start();
                    return listener;
                }
                catch (Exception exception)
                {
                    Log.Write("Cannot start listener on address: {0}".Combine(listener.LocalEndpoint));

                    Log.Write(exception);
                    return null;
                }
            }).SkipDefault();

            listenerThreads = startedListeners.Select(listener => SharedObjects.AsyncInvoker.Invoke(MainLoop, listener)).ToReadOnlySet();
        }

        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]//Всё тестируется, ошибок не будет
        private void MainLoop(TcpListener listener)
        {
            while (!Disposed)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();

                    if (Disposed)//Если вызван Stop
                        return;

                    threadPool.EnqueueAction(() => ProcessClientWithTimeout(client));
                }
                catch (Exception ex)
                {
                    if (Disposed)//Если вызван Stop
                        return;

                    Log.Write(ex);

                    Thread.Sleep(10000);//Спим 10 секунд, чтобы избежать потери производительности при большом количестве сетевых исключений
                }
            }
        }

        private void ProcessClientWithTimeout(TcpClient client)
        {
            var waiter = SharedObjects.AsyncInvoker.Invoke(ProcessClient, client);

            if (waiter.Wait(maxOperationTimeout))
                return;

            client.Close();

            waiter.Abort();

            throw new TimeoutException("Timeout exceeded during processing request from {0}".Combine(client.Client.RemoteEndPoint));
        }

        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        private void ProcessClient(TcpClient client)
        {
            Check.ObjectIsNotNull(client, "client");

            using (client)
            {
                Stopwatch timer = Stopwatch.StartNew();

                NetworkStream stream = client.GetStream();

                using (var reader = new BinaryReader(stream))
                using (var writer = new BinaryWriter(stream))
                {
                    string serverName = reader.ReadString();

                    BaseServer implementation = registeredServers.TryGetValue(serverName);

                    if (implementation == null)
                    {
                        Log.Write("Unable to resolver server {0}", serverName);

                        return;
                    }

                    Log.Add("[{1}]Processing request from {0} started", client.Client.RemoteEndPoint, implementation.GetType().Name);

                    implementation.ProcessNetworkRequest(reader, writer);

                    Log.Add(
                        "[{2}]Processing request from {0} finished. Execution time (milliseconds): {1}", client.Client.RemoteEndPoint, timer.ElapsedMilliseconds,
                        implementation.GetType().Name);
                }
            }
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();

            listeners.ForEach(listener => listener.Stop());

            registeredServers.Clear();

            foreach (IWaitHandle listenerThread in listenerThreads)//Ждем окончания завершения работы всех операций
            {
                try
                {
                    listenerThread.Wait(maxOperationTimeout);

                    listenerThread.Abort();
                }
                catch (Exception ex)
                {
                    Log.Write("Unable to stop one of the listeners: " + ex);
                }
            }

            DisposeSafe(threadPool);
        }

        public void RegisterServer(BaseServer server)
        {
            if (!registeredServers.TryAdd(server.Name, server))
                throw new ArgumentException("The server with the name {0} had already been added".Combine(server.Name));
        }
    }
}