using Godot;
public static class FilePathUtils
{
    public static string GlobalizePath(string path)
    {
        if (OS.HasFeature("editor"))
        {
            return ProjectSettings.GlobalizePath(path);
        }
        else
        {
            return OS.GetExecutablePath().GetBaseDir().PathJoin(path.Substring(6)); // substring removes "res://" at beginning
        }
    }
}