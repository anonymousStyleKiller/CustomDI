using System.Reflection;
using DI.Descriptors;
using DI.Interfaces;

namespace DI.Models.ActivationBuilder;

public class ReflectionBasedActivationBuilder : BaseActivationBuilder
{
    protected override Func<IScope, object> BuildActivationInternal(TypeBasedServiceDescriptor typeBasedServiceDescriptor, ConstructorInfo constructor,
        ParameterInfo[] parameters, ServiceDescriptor descriptor)
    {
        var tb = (TypeBasedServiceDescriptor)descriptor;

        constructor = tb.ImplementationType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Single();
        parameters = constructor.GetParameters();
        return s =>
        {
            var parametersForConstructor = new object[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                parametersForConstructor[i] = s.Resolve(parameters[i].ParameterType);
            }

            return constructor.Invoke(parametersForConstructor);
        };
    }
}