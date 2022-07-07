using DI.Interfaces;
using DI.Models;

namespace DI.Descriptors;

public class FactoryBasedServiceDescriptor : ServiceDescriptor
{
    public Func<IScope, object> Factory { get; init; }
}