using System;

namespace DashTheDev.SDTD.ModCore;

internal class ContainerRegistration(Type implementationType, LifetimeType lifetimeType)
{
    public Type ImplementationType { get; } = implementationType;
    public LifetimeType LifetimeType { get; } = lifetimeType;
}
