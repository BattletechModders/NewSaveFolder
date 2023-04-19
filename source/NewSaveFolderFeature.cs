using System;
using System.IO;
using BattleTech;

namespace NewSaveFolder;

internal class NewSaveFolderFeature
{
    internal static string SavesPath => Path.GetFullPath(Path.Combine(BattleTechPath, Control.settings.Path));

    private static readonly string BattleTechPath = GetBattleTechPath();
    private static string GetBattleTechPath()
    {
        var manifestDirectory = Path.GetDirectoryName(VersionManifestUtilities.MANIFEST_FILEPATH);
        if (manifestDirectory == null)
        {
            throw new InvalidOperationException();
        }

        return Path.GetFullPath(
            Path.Combine(manifestDirectory,
                Path.Combine(
                    Path.Combine(
                        "..",
                        ".."
                    ),
                    ".."
                )
            )
        );
    }
}