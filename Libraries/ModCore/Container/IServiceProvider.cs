using System;

namespace DashTheDev.SDTD.ModCore;

public interface IServiceProvider
{
    public T GetService<T>();
    public object GetService(Type serviceType);
}