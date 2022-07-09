using DI.Descriptors;
using DI.Enums;
using DI.Interfaces;

namespace DI.Extensions;

public static class ContainerBuilderExtensions
{
    private static IContainerBuilder RegisterType(this IContainerBuilder builder, Type service,
        Type serviceImplementation, LifeTime lifeTime)
    {
        builder.Register(new TypeBasedServiceDescriptor
        {
            ImplementationType = serviceImplementation,
            ServiceType = service,
            LifeTime = lifeTime
        });
        return builder;
    }

    private static IContainerBuilder RegisterFactory(this IContainerBuilder builder, Type service,
        Func<IScope, object> factory,
        Type serviceImplementation, LifeTime lifeTime)
    {
        builder.Register(new FactoryBasedServiceDescriptor
            { Factory = factory, ServiceType = service, LifeTime = lifeTime });
        return builder;
    }

    private static IContainerBuilder RegisterInstance(this IContainerBuilder builder, Type service, object instance)
    {
        builder.Register(new InstanceBasedServiceDescriptor(service, instance));
        return builder;
    }


    #region Base

    public static IContainerBuilder RegisterSingleton(this IContainerBuilder builder, Type service,
        Type serviceImplementation)
        => RegisterType(builder, service, serviceImplementation, LifeTime.Singleton);

    public static IContainerBuilder RegisterTransient(this IContainerBuilder builder, Type service,
        Type serviceImplementation)
        => RegisterType(builder, service, serviceImplementation, LifeTime.Transient);

    public static IContainerBuilder RegisterScoped(this IContainerBuilder builder, Type service,
        Type serviceImplementation)
        => RegisterType(builder, service, serviceImplementation, LifeTime.Scoped);

    #endregion

    #region Generic

    public static IContainerBuilder RegisterSingleton<TService, TImplementation>(this IContainerBuilder builder)
        where TImplementation : TService
        => RegisterType(builder, typeof(TService), typeof(TImplementation), LifeTime.Singleton);

    public static IContainerBuilder RegisterTransient<TService, TImplementation>(this IContainerBuilder builder)
        where TImplementation : TService
        => RegisterType(builder, typeof(TService), typeof(TImplementation), LifeTime.Transient);

    public static IContainerBuilder RegisterScoped<TService, TImplementation>(this IContainerBuilder builder)
        where TImplementation : TService
        => RegisterType(builder, typeof(TService), typeof(TImplementation), LifeTime.Scoped);

    #endregion

    #region Factory

    public static IContainerBuilder RegisterSingleton(this IContainerBuilder builder, Type service,
        Func<IScope, object> factory,
        Type serviceImplementation)
        => RegisterFactory(builder, service, factory, serviceImplementation, LifeTime.Singleton);

    public static IContainerBuilder RegisterTransient(this IContainerBuilder builder, Type service,
        Func<IScope, object> factory,
        Type serviceImplementation)
        => RegisterFactory(builder, service, factory, serviceImplementation, LifeTime.Transient);

    public static IContainerBuilder RegisterScoped(this IContainerBuilder builder, Type service,
        Func<IScope, object> factory,
        Type serviceImplementation)
        => RegisterFactory(builder, service, factory, serviceImplementation, LifeTime.Scoped);

    #endregion

    #region InstanceSingleton

    public static IContainerBuilder RegisterSingleton<T>(this IContainerBuilder builder, object instance)
        => builder.RegisterInstance(typeof(T), instance);

    #endregion
}