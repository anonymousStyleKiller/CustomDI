using System.Reflection;
using DI.Descriptors;
using DI.Interfaces;

namespace DI.Models.ActivationBuilder;

public abstract class BaseActivationBuilder : IActivationBuilder
{
    public Func<IScope, object> BuildActivation(ServiceDescriptor descriptor)
    {
        var typeBasedServiceDescriptor = (TypeBasedServiceDescriptor)descriptor;

        var constructor = typeBasedServiceDescriptor.ImplementationType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Single();
        var parameters = constructor.GetParameters();
        return BuildActivationInternal(typeBasedServiceDescriptor, constructor, parameters, descriptor);
    }

    protected abstract Func<IScope, object> BuildActivationInternal(TypeBasedServiceDescriptor typeBasedServiceDescriptor, ConstructorInfo constructor, ParameterInfo[] parameters, ServiceDescriptor descriptor);
}