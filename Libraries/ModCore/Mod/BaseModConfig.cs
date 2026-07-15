using System.IO;

namespace DashTheDev.SDTD.ModCore;

public interface IModConfig
{
    public bool IsDebug { get; set; }
    public void SetBaseFilePath(string path);
    public void Load();
    public void Save();
}

public abstract class BaseModConfig : IModConfig
{
    protected string BaseFilePath { get; private set; }
    protected virtual string FileName { get; } = "ModConfig.xml";
    protected string FilePath => Path.Combine(BaseFilePath, FileName);

    public bool IsDebug { get; set; }

    public void SetBaseFilePath(string path)
    {
        BaseFilePath = path;
    }

    public abstract void Load();
    public abstract void Save();
}