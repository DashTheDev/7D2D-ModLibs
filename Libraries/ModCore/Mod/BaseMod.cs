using HarmonyLib;

namespace DashTheDev.SDTD.ModCore;

public abstract class BaseMod<TMod, TModConfig>
    where TMod : BaseMod<TMod, TModConfig>, IModApi
    where TModConfig : IModConfig, new()
{
    private readonly Container _container = new();
    public static TMod Instance { get; private set; }
    public IServiceProvider ServiceProvider => _container;

    #region Service helpers

    private TModConfig? _config;
    public TModConfig Config => _config ??= GetService<TModConfig>();

    private IModLogger? _logger;
    public IModLogger Logger => _logger ??= GetService<IModLogger>();

    #endregion

    public virtual void InitMod(Mod gameMod)
    {
        Instance = (TMod)this;
        SetupContainer(gameMod);
        ApplyHarmonyPatches(gameMod);
    }

    private TModConfig SetupConfig(Mod gameMod)
    {
        TModConfig config = new();
        config.SetBaseFilePath(gameMod.Path);
        config.Load();
        return config;
    }

    private void SetupContainer(Mod gameMod)
    {
        TModConfig config = SetupConfig(gameMod);
        _container.AddSingleton<IModConfig, TModConfig>(config);
        _container.AddSingleton(config);
        _container.AddSingleton(gameMod);
        AddServices(_container);
    }

    public virtual void ApplyHarmonyPatches(Mod gameMod)
    {
        Harmony harmony = new(gameMod.Name);
        harmony.PatchAll(GetType().Assembly);
    }

    protected virtual void AddServices(IServiceCollection services)
    {
        services.AddSingleton<IModLogger, ModLogger>();
    }

    protected T GetService<T>()
    {
        return ServiceProvider.GetService<T>();
    }
}