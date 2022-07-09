using DI.Interfaces;

namespace DI.Models;

public class Container : IContainer
{
    private Dictionary<Type, ServiceDescriptor> Descriptors { get; }

    public Container(IEnumerable<ServiceDescriptor> descriptors)
    {
        Descriptors = descriptors.ToDictionary(x => x.ServiceType);
    }

    public IScope CreateScope()
    {
        return new Scope(this);
    }

    public object CreateInstance(Type service)
    {
        if (!Descriptors.TryGetValue(service, out _))
            throw new InvalidOperationException($"Service {service} is not registered");

        var constructorInfo = service.GetConstructors().Single();
        var parameters = constructorInfo.GetParameters();
        var parametersForConstructor = new object[parameters.Length];
        for (var i = 0; i < parameters.Length; i++)
        {
            parametersForConstructor[i] = CreateInstance(parameters[i].ParameterType);
        }

        return constructorInfo.Invoke(parametersForConstructor);
    }
}