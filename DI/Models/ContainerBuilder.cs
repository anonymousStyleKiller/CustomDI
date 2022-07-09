using DI.Interfaces;

namespace DI.Models;

class ContainerBuilder : IContainerBuilder
{
    private List<ServiceDescriptor> _descriptors = new();
    public void Register(ServiceDescriptor serviceDescriptor)
    {
        _descriptors.Add(serviceDescriptor);
    }

    public IContainer Build()
    {
        return new Container(_descriptors);
    }
}