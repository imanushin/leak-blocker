using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Server.Service.InternalTools.LocalStorages;

namespace LeakBlocker.Server.Service.InternalTools.SingletonImplementation.LocalStorages
{
    internal abstract class BaseConfigurationManager<TObject> : IBaseConfigurationManager<TObject> where TObject : BaseReadOnlyObject
    {
        private readonly string saveDirectory;
        private readonly string filePath;

        private TObject currentObject;

        private readonly object syncRoot = new object();

        protected object SyncRoot
        {
            get
            {
                return syncRoot;
            }
        }

        protected BaseConfigurationManager(string fileName)
        {
            Check.StringIsMeaningful(fileName, "fileName");

            saveDirectory = SharedObjects.Constants.UserDataFolder;
            filePath = Path.Combine(saveDirectory, fileName);

            currentObject = Load();
        }

        public virtual void Save(TObject obj)
        {
            Check.ObjectIsNotNull(obj, "obj");

            using (new TimeMeasurement("{0}::Save".Combine(GetType().Name), true))
            {
                lock (syncRoot)
                {
                    if (!Directory.Exists(saveDirectory))
                        Directory.CreateDirectory(saveDirectory);

                    using (FileStream file = File.Open(filePath, FileMode.Create))
                    {
                        obj.SerializeToXml(file);
                    }

                    currentObject = obj;
                }
            }
        }

        public void SaveIfDifferent(TObject obj)
        {
            Check.ObjectIsNotNull(obj, "obj");

            lock (syncRoot)
            {
                if (Equals(obj, Current))
                    return;

                Save(obj);
            }
        }

        private TObject Load()
        {
            using (new TimeMeasurement("{0}::Load".Combine(GetType().Name), true))
            {
                lock (syncRoot)
                {
                    if (!Directory.Exists(saveDirectory))
                        Directory.CreateDirectory(saveDirectory);

                    if (!File.Exists(filePath))
                        return null;

                    using (var file = File.OpenRead(filePath))
                    {
                        return BaseObjectSerializer.DeserializeFromXml<TObject>(file);
                    }
                }
            }
        }

        public virtual TObject Current
        {
            get
            {
                lock (syncRoot)
                {
                    return currentObject;
                }
            }
        }
    }
}
