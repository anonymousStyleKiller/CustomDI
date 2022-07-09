using DI.Interfaces;

namespace DI.Models;

public class Scope : IScope
{
    private readonly Container _container;

    public Scope(Container container)
    {
        _container = container;
    }

    public object Resolve(Type service) => _container.CreateInstance(service);
}