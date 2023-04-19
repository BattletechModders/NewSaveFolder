using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BattleTech.Save;
using UnityEngine;

namespace NewSaveFolder.Patches;

[HarmonyPatch]
public static class CachedSettings_Patches
{
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(CachedSettings), nameof(CachedSettings.Settings), MethodType.Setter)]
    [HarmonyPatch(typeof(CachedSettings), nameof(CachedSettings.Settings), MethodType.Getter)]
    [HarmonyPatch(typeof(CachedSettings), nameof(CachedSettings.SaveSettingsToPlayerPrefs))]
    [HarmonyPatch(typeof(CachedSettings), nameof(CachedSettings.ClearCachedSettings))]
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return instructions
            .MethodReplacer(
                AccessTools.Method(typeof(PlayerPrefs), nameof(PlayerPrefs.HasKey)),
                AccessTools.Method(typeof(NewSaveFolderFeature), nameof(HasKey))
            )
            .MethodReplacer(
                AccessTools.Method(typeof(PlayerPrefs), nameof(PlayerPrefs.GetString), new []{typeof(string), typeof(string)}),
                AccessTools.Method(typeof(NewSaveFolderFeature), nameof(GetString))
            )
            .MethodReplacer(
                AccessTools.Method(typeof(PlayerPrefs), nameof(PlayerPrefs.DeleteKey)),
                AccessTools.Method(typeof(NewSaveFolderFeature), nameof(DeleteKey))
            )
            .MethodReplacer(
                AccessTools.Method(typeof(PlayerPrefs), nameof(PlayerPrefs.SetString)),
                AccessTools.Method(typeof(NewSaveFolderFeature), nameof(SetString))
            );
    }

    private static bool HasKey(string key)
    {
        return File.Exists(PathByKey(key));
    }

    private static string GetString(string key, string defaultValue)
    {
        try
        {
            if (HasKey(key))
            {
                return File.ReadAllText(PathByKey(key));
            }
        }
        catch (Exception e)
        {
            Control.Logger.LogError(e);
        }
        return defaultValue;
    }

    private static void DeleteKey(string key)
    {
        try
        {
            if (HasKey(key))
            {
                File.Delete(PathByKey(key));
            }
        }
        catch (Exception e)
        {
            Control.Logger.LogError(e);
        }
    }

    private static void SetString(string key, string value)
    {
        try
        {
            var path = PathByKey(key);
            var dir = Path.GetDirectoryName(path);
            Directory.CreateDirectory(dir ?? throw new InvalidOperationException());
            File.WriteAllText(path, value, Encoding.UTF8);
        }
        catch (Exception e)
        {
            Control.Logger.LogError(e);
        }
    }

    private static string PathByKey(string key)
    {
        return Path.Combine(NewSaveFolderFeature.SavesPath, key + ".pref");
    }
}