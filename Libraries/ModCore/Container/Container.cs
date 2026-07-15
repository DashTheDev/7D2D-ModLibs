using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DashTheDev.SDTD.ModCore;

internal class Container : IServiceCollection, IServiceProvider
{
    #region Fields

    private readonly Dictionary<Type, ContainerRegistration> _registrations = [];
    private readonly Dictionary<Type, object> _singletons = [];

    #endregion

    #region Private Methods

    private void Register<TService, TImplementation>(LifetimeType lifetime, TImplementation? instance = default)
        where TImplementation : TService
    {
        Type serviceType = typeof(TService);

        if (_registrations.ContainsKey(serviceType))
        {
            throw new InvalidOperationException($"{serviceType.Name} is already registered in container.");
        }

        ContainerRegistration registration = new(typeof(TImplementation), lifetime);
        _registrations.Add(serviceType, registration);

        if (lifetime == LifetimeType.Singleton && instance is not null)
        {
            _singletons.Add(serviceType, instance);
        }
    }

    private object Resolve(Type type)
    {
        if (!_registrations.TryGetValue(type, out ContainerRegistration registration))
        {
            throw new Exception($"{type} is not registered in container.");
        }

        if (registration.LifetimeType == LifetimeType.Singleton)
        {
            if (_singletons.TryGetValue(type, out object instance))
            {
                return instance;
            }

            instance = CreateInstance(registration);
            _singletons.Add(type, instance);
            return instance;
        }

        return CreateInstance(registration);
    }

    private object CreateInstance(ContainerRegistration registration)
    {
        // TODO: Obviously this isn't ideal. Should look for biggest constructor or [Inject] decorated
        ConstructorInfo contructor = registration.ImplementationType.GetConstructors().FirstOrDefault();

        if (contructor is null)
        {
            throw new Exception($"Constructor for {registration} could not be found.");
        }

        object[] args = contructor
            .GetParameters()
            .Select(p => Resolve(p.ParameterType))
            .ToArray();

        return Activator.CreateInstance(registration.ImplementationType, args);
    }

    #endregion

    #region IServiceCollection

    public void AddSingleton<T>()
        => Register<T, T>(LifetimeType.Singleton);

    public void AddSingleton<T>(T instance)
        => Register<T, T>(LifetimeType.Singleton, instance);

    public void AddSingleton<TService, TImplementation>() where TImplementation : TService
        => Register<TService, TImplementation>(LifetimeType.Singleton);

    public void AddSingleton<TService, TImplementation>(TImplementation instance) where TImplementation : TService
        => Register<TService, TImplementation>(LifetimeType.Singleton, instance);

    public void AddTransient<T>()
        => Register<T, T>(LifetimeType.Transient);

    public void AddTransient<TService, TImplementation>() where TImplementation : TService
        => Register<TService, TImplementation>(LifetimeType.Transient);

    #endregion

    #region IServiceProvider

    public T GetService<T>()
    {
        Type type = typeof(T);

        if (GetService(type) is not T instance)
        {
            throw new Exception($"{type} is not registered in container.");
        }

        return instance;
    }

    public object GetService(Type serviceType)
    {
        return Resolve(serviceType);
    }

    #endregion
}
