using DI.Models;

namespace DI.Interfaces;

public interface IActivationBuilder
{
    public Func<IScope, object> BuildActivation(ServiceDescriptor descriptor);
}