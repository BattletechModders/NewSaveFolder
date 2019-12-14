using Harmony;
using HBS.Logging;
using HBS.Util;
using System;
using System.Linq;
using System.Reflection;

namespace NewSaveFolder
{
    public static class Control
    {
        internal static string Name = nameof(NewSaveFolder);
        internal static ILog Logger => HBS.Logging.Logger.GetLogger(Name);
        internal static NewSaveFolderSettings settings = new NewSaveFolderSettings();

        public static void Start(string modDirectory, string json)
        {
            try
            {
                var hasModTek = GetAssemblyByName("ModTek") != null;
                if (!hasModTek)
                {
                    throw new InvalidOperationException("This mod is for ModTek only and does not run under ModLoader");
                }
                JSONSerializationUtility.FromJSON(settings, json);
                if (settings.MigrateSavesToVanillaLocation)
                {
                    throw new InvalidOperationException("MigrateSavesToVanillaLocation not yet implemented");
                    // TODO how to know what location to use?
                    // cloud vs local
                    // gog vs steam vs more?
                }
                else
                {
                    Control.Logger.Log($"Saving to folder {NewSaveFolderFeature.SavesPath}");
			        var harmony = HarmonyInstance.Create(Name);
                    harmony.PatchAll(Assembly.GetExecutingAssembly());
                }
            }
            catch (Exception e)
            {
                Control.Logger.LogError(e);
            }
        }

        private static Assembly GetAssemblyByName(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == name);
        }
    }
}
