using DI.Enums;

namespace DI.Models;

public class ServiceDescriptor
{
    public Type ServiceType { get; init; }
    public LifeTime LifeTime { get; init; }
}