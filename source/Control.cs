using HBS.Logging;
using HBS.Util;
using System;
using System.Reflection;

namespace NewSaveFolder;

public static class Control
{
    internal static ILog Logger => HBS.Logging.Logger.GetLogger(nameof(NewSaveFolder));
    internal static readonly NewSaveFolderSettings settings = new();

    public static void Start(string modDirectory, string json)
    {
        try
        {
            JSONSerializationUtility.FromJSON(settings, json);
            if (settings.MigrateSavesToVanillaLocation)
            {
                throw new InvalidOperationException("MigrateSavesToVanillaLocation not yet implemented");
                // TODO how to know what location to use?
                // cloud vs local
                // gog vs steam vs more?
            }

            Logger.Log($"Saving to folder {NewSaveFolderFeature.SavesPath}");
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), nameof(NewSaveFolder));
        }
        catch (Exception e)
        {
            Logger.LogError(e);
            throw;
        }
    }
}