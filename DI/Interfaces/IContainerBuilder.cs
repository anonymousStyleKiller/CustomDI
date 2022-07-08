using DI.Models;

namespace DI.Interfaces;

public interface IContainerBuilder
{
    void Register(ServiceDescriptor serviceDescriptor);
    IContainer Build();
}