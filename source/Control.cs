using Harmony;
using HBS.Logging;
using HBS.Util;
using System;
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
                var isModTek = Type.GetType("ModTek.ModTek") != null;
                if (!isModTek)
                {
                    throw new InvalidOperationException("This mod is for ModTek only and does not run under ModLoader");
                }
                JSONSerializationUtility.FromJSON(settings, json);
			    var harmony = HarmonyInstance.Create(Name);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception e)
            {
                Control.Logger.LogError(e);
            }
        }
    }
}
