namespace DI.Interfaces;

internal interface IContainer
{
    IScope CreateScope();
}