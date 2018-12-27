using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools
{
    /// <summary>
    /// Performace counter. Also writes information to the log.
    /// </summary>
    public sealed class TimeMeasurement : Disposable
    {
        private readonly string name;
        private readonly bool optional;
        private readonly Stopwatch stopwatch = new Stopwatch();
        private volatile bool disposed;

        /// <summary>
        /// Creates an instance if the TimeMeasurement class.
        /// </summary>
        /// <param name="name">Object name. Affects only on the text in the log.</param>
        /// <param name="optional">Message will be written only if detailed log is turned on.</param>
        public TimeMeasurement(string name, bool optional = false)
        {
            this.name = name ?? ("TimeMeasurement_" + Guid.NewGuid());
            this.optional = optional;

            string message = name + " started.";
            if (optional)
                Log.Add(message);
            else
                Log.Write(message);

            stopwatch.Start();
        }

        /// <summary>
        /// Stops counting performance.
        /// </summary>
        protected override void DisposeManaged()
        {
            if (disposed)
                return;
            disposed = true;
            
            stopwatch.Stop();

            string message = name + " completed (ran for {0} milliseconds).".Combine(stopwatch.ElapsedMilliseconds);
            if (optional)
                Log.Add(message);
            else
                Log.Write(message);
        }
    }
}
