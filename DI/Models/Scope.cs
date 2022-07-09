using System.Collections.Concurrent;
using DI.Enums;
using DI.Interfaces;

namespace DI.Models;

public class Scope : IScope
{
    private readonly Container _container;
    private readonly ConcurrentDictionary<Type, object> _scopedInstances = new();

    public Scope(Container container)
    {
        _container = container;
    }

    public object Resolve(Type service)
    {
        var descriptor = _container.FindDescriptor(service);
        if (descriptor.LifeTime == LifeTime.Transient)
            return _container.CreateInstance(service, this);
        if (descriptor.LifeTime is LifeTime.Scoped || _container._rootScope == this)
            return _scopedInstances.GetOrAdd(service, s => _container.CreateInstance(s, this));
        if (descriptor.LifeTime == LifeTime.Singleton)
            return _container._rootScope.Resolve(service);
        return _container.CreateInstance(service, this);
    }
}