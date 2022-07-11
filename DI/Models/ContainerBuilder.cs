using DI.Interfaces;
using DI.Models.ActivationBuilder;

namespace DI.Models;

class ContainerBuilder : IContainerBuilder
{
    private readonly IActivationBuilder _builder;
    private readonly List<ServiceDescriptor> _descriptors = new();

    public ContainerBuilder(IActivationBuilder builder)
    {
        _builder = builder;
    }

    public void Register(ServiceDescriptor serviceDescriptor)
    {
        _descriptors.Add(serviceDescriptor);
    }

    public IContainer Build()
    {
        return new Container(_descriptors, new LambdaActivationBuild());
    }
}