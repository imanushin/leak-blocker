using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Storage.InternalTools;
using LeakBlocker.Server.Installer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LeakBlocker.Libraries.Storage.Tests
{
    [TestClass]
    public abstract class BaseStorageTest : BaseTest
    {
        private const string databasePassword = "Ytrewq21";

        private string databasePath;

        private const string databaseName = "TestDatabase";
        private const string databaseFolder = ".\\TempDatabaseFolder";

        private const string precreatedDatabaseName = "Precreated.sdf";

        private readonly static string precreatedDatabaseFolder = Path.Combine(databaseFolder, "Precreated\\");
        private readonly static string precreatedDatabasePath = Path.Combine(precreatedDatabaseFolder, precreatedDatabaseName);

        private readonly static IDatabaseInitializer databaseInitializer = new DatabaseInitializer()
        {
            DatabasePassword = databasePassword
        };

        //private static bool isInitialized;

        /// <summary>
        /// Only internal use
        /// </summary>
        internal BaseStorageTest()
        {
        }

        [AssemblyInitialize]
        public static void GlobalInitialize(TestContext testContext)
        {
            SharedObjects.Singletons.Constants.SetTestImplementation(new ConstantsStub());
            StorageObjects.Singletons.CrossRequestCache.SetTestImplementation(new CrossRequestCache());
            StorageObjects.Singletons.IndexInitializer.SetTestImplementation(new IndexInitializer());
            StorageObjects.Singletons.DatabaseConstants.SetTestImplementation(new PrecreatedDbContants());
            StorageObjects.Singletons.DatabaseInitializer.SetTestImplementation(databaseInitializer);

            if (!Directory.Exists(precreatedDatabaseFolder))
                Directory.CreateDirectory(precreatedDatabaseFolder);

            if (File.Exists(precreatedDatabasePath))
                File.Delete(precreatedDatabasePath);

            if (!File.Exists(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\System.Data.SqlServerCe.dll"))
            {
                string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";

                File.WriteAllBytes(folder + "sqlcese40.dll", Environment.Is64BitOperatingSystem ? SQL.sqlcese40_64 : SQL.sqlcese40_32);
                File.WriteAllBytes(folder + "sqlceqp40.dll", Environment.Is64BitOperatingSystem ? SQL.sqlceqp40_64 : SQL.sqlceqp40_32);
                File.WriteAllBytes(folder + "sqlceme40.dll", Environment.Is64BitOperatingSystem ? SQL.sqlceme40_64 : SQL.sqlceme40_32);
                File.WriteAllBytes(folder + "sqlceer40EN.dll", Environment.Is64BitOperatingSystem ? SQL.sqlceer40EN_64 : SQL.sqlceer40EN_32);
                File.WriteAllBytes(folder + "sqlcecompact40.dll", Environment.Is64BitOperatingSystem ? SQL.sqlcecompact40_64 : SQL.sqlcecompact40_32);
                File.WriteAllBytes(folder + "sqlceca40.dll", Environment.Is64BitOperatingSystem ? SQL.sqlceca40_64 : SQL.sqlceca40_32);
                File.WriteAllBytes(folder + "msvcr90.dll", Environment.Is64BitOperatingSystem ? SQL.msvcr90_64 : SQL.msvcr90_32);
                File.WriteAllBytes(folder + "Microsoft.VC90.CRT.manifest", Environment.Is64BitOperatingSystem ? SQL.manifest_64 : SQL.manifest_32);
                File.WriteAllBytes(folder + "System.Data.SqlServerCe.dll", SQL.System_Data_SqlServerCe);
                File.WriteAllBytes(folder + "System.Data.SqlServerCe.Entity.dll", SQL.System_Data_SqlServerCe_Entity);
            }

            using (new DatabaseModel())
            {
            }

            //isInitialized = true;
        }


        [TestInitialize]
        public void InitializeDatabase()
        {
            Directory.EnumerateFiles(databaseFolder, "*.sdf").ForEach(
                fileName =>
                {
                    try
                    {
                        File.Delete(fileName);
                    }
                    catch
                    {
                    }
                });


            databasePath = Path.Combine(databaseFolder, "Test database " + Guid.NewGuid() + ".sdf");

            File.Copy(precreatedDatabasePath, databasePath);

            var constants = Mocks.StrictMock<IDatabaseConstants>();

            constants.Stub(x => x.DatabaseName).Return(databaseName).Repeat.Any();
            constants.Stub(x => x.DatabasePath).Return(databasePath).Repeat.Any();
            constants.Stub(x => x.DatabaseFolder).Return(databaseFolder).Repeat.Any();

            StorageObjects.Singletons.DatabaseConstants.SetTestImplementation(constants);
            StorageObjects.Singletons.CrossRequestCache.SetTestImplementation(new CrossRequestCache());//Каждый раз делаем новый кеш, так как база данных меняется
            StorageObjects.Singletons.IndexInitializer.SetTestImplementation(new IndexInitializer());
            StorageObjects.Factories.DatabaseModel.EnqueueConstructor(() => new DatabaseModel());
            StorageObjects.Singletons.DatabaseInitializer.SetTestImplementation(databaseInitializer);
        }

        [TestCleanup]
        public void CleanupDatabase()
        {
            if (File.Exists(databasePath))
                File.Delete(databasePath);
        }


        private sealed class PrecreatedDbContants : IDatabaseConstants
        {
            public string DatabaseName
            {
                get
                {
                    return precreatedDatabaseName;
                }
            }

            public string DatabasePath
            {
                get
                {
                    return precreatedDatabasePath;
                }
            }

            public string DatabaseFolder
            {
                get
                {
                    return precreatedDatabaseFolder;
                }
            }
        }

    }
}
