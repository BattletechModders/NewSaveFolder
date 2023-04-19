using System.IO;
using BattleTech.Save.Core;

namespace NewSaveFolder.Patches;

[HarmonyPatch(typeof(WriteLocation), MethodType.Constructor, typeof(string), typeof(bool))]
public static class WriteLocation_Constructor_Patch
{
    [HarmonyPrefix]
    [HarmonyWrapSafe]
    public static void Prefix(ref bool __runOriginal, ref string rootPath, ref bool usePlatform)
    {
        if (!__runOriginal)
        {
            return;
        }

        var fullPath = Path.GetFullPath(rootPath).TrimEnd(Path.DirectorySeparatorChar);
        var dirName = Path.GetFileName(fullPath);
        var platformPath = PathByPlatform(usePlatform);
        rootPath = Path.Combine(platformPath, dirName);
        usePlatform = false;
    }

    private static string PathByPlatform(bool usePlatform)
    {
        return Path.Combine(NewSaveFolderFeature.SavesPath, usePlatform ? "cloud" : "local");
    }
}