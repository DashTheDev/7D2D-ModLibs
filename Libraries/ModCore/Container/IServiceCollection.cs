namespace DashTheDev.SDTD.ModCore;

public interface IServiceCollection
{
    public void AddSingleton<T>();
    public void AddSingleton<T>(T instance);
    public void AddSingleton<TService, TImplementation>() where TImplementation : TService;
    public void AddSingleton<TService, TImplementation>(TImplementation instance) where TImplementation : TService;
    public void AddTransient<T>();
    public void AddTransient<TService, TImplementation>() where TImplementation : TService;
}