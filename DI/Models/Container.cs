using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;
using DI.Descriptors;
using DI.Interfaces;

namespace DI.Models;

public class Container : IContainer, IDisposable, IAsyncDisposable
{
    private readonly IActivationBuilder _builder;
    private ImmutableDictionary<Type, ServiceDescriptor> Descriptors { get; }
    private readonly ConcurrentDictionary<Type, Func<IScope, object>> _buildActivator = new();
    public readonly Scope _rootScope;

    public Container(IEnumerable<ServiceDescriptor> descriptors, IActivationBuilder builder)
    {
        _builder = builder;
        Descriptors = descriptors.ToImmutableDictionary(x => x.ServiceType);
        _rootScope = new Scope(this);
    }

    public IScope CreateScope()
    {
        return new Scope(this);
    }

    private Func<IScope, object> BuildActivation(Type service, IScope scope)
    {
        if (!Descriptors.TryGetValue(service, out var descriptor))
            throw new InvalidOperationException($"Service {service} is not registered");

        if (descriptor is InstanceBasedServiceDescriptor ib) return _ => ib.Instance;
        if (descriptor is FactoryBasedServiceDescriptor fb) return _ => fb.Factory(scope);
        return _builder.BuildActivation(descriptor);
    }

    public object CreateInstance(Type service, IScope scope)
    {
        return _buildActivator.GetOrAdd(service, type => BuildActivation(type, scope));
    }

    public ServiceDescriptor FindDescriptor(Type service)
    {
        Descriptors.TryGetValue(service, out var result);
        return result;
    }

    public void Dispose()
    {
        _rootScope.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _rootScope.DisposeAsync();
    }
}