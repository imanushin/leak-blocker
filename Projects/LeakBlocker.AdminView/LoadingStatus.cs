using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.AdminView
{
    internal static class LoadingStatus
    {
        internal static void TriggerLoadingCompleted()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                Type type = assembly.GetType("LeakBlocker.EntryPoint");
                if (type == null)
                    continue;

                FieldInfo fieldReference = type.GetField("loaded", BindingFlags.Static | BindingFlags.NonPublic);
                if (fieldReference == null)
                    Log.Write("Cannot get field 'LeakBlocker.EntryPoint.loaded'.");
                else
                    fieldReference.SetValue(null, true);

                return;
            }

            Log.Write("Cannot find type 'LeakBlocker.EntryPoint'.");
        }
    }
}
