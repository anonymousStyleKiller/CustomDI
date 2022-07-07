using DI.Models;

namespace DI.Interfaces;

internal interface IContainerBuilder
{
    void Register(ServiceDescriptor serviceDescriptor);
    IContainer Build();
}