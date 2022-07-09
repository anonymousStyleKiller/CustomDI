namespace DI.Interfaces;

public interface IContainer : IDisposable, IAsyncDisposable
{
    IScope CreateScope();
}