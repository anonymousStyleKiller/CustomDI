namespace DI.Interfaces;

public interface IScope
{
    object Resolve(Type service);
}