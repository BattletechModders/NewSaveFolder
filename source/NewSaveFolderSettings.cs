namespace NewSaveFolder;

internal class NewSaveFolderSettings
{
    public const string PathDescription = "Redirects all save operations to use one folder for all save related data.";
    public string Path = "Mods/Saves";

    public bool MigrateSavesToVanillaLocation = false;
}