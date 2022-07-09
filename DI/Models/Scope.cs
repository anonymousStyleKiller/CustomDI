using System.Collections.Concurrent;
using DI.Enums;
using DI.Interfaces;

namespace DI.Models;

public class Scope : IScope
{
    private readonly Container _container;
    private readonly ConcurrentDictionary<Type, object> _scopedInstances = new();
    private readonly ConcurrentStack<object> _disposables = new();

    public Scope(Container container)
    {
        _container = container;
    }

    public object Resolve(Type service)
    {
        var descriptor = _container.FindDescriptor(service);
        if (descriptor.LifeTime == LifeTime.Transient)
            return CreateInstanceInternal(service);
        if (descriptor.LifeTime is LifeTime.Scoped || _container._rootScope == this)
            return _scopedInstances.GetOrAdd(service, s => CreateInstanceInternal(service));
        if (descriptor.LifeTime == LifeTime.Singleton)
            return _container._rootScope.Resolve(service);
        return _container.CreateInstance(service, this);
    }


    private object CreateInstanceInternal(Type service)
    {
        var result = _container.CreateInstance(service, this);
        if (result is IDisposable or IAsyncDisposable)
            _disposables.Push(result);
        return result;
    }

    public void Dispose()
    {
        foreach (var disposable in _disposables)
        {
            if(disposable is IDisposable d)
                d.Dispose();
            else if(disposable is IAsyncDisposable ad)
                ad.DisposeAsync().GetAwaiter().GetResult();
        }
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var disposable in _disposables)
        {
            if(disposable is IAsyncDisposable d)
               await d.DisposeAsync();
            else if(disposable is IDisposable ad)
                ad.Dispose();
        }
    }
}