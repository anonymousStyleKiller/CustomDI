namespace DI.Interfaces;

public interface IContainer
{
    IScope CreateScope();
}