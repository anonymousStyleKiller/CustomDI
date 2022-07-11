using Autofac;
using BenchmarkDotNet.Attributes;
using DI.Controllers;
using DI.Extensions;
using DI.Interfaces;
using DI.Interfaces.Services;
using DI.Models.ActivationBuilder;
using DI.Services;
using Microsoft.Extensions.DependencyInjection;
using ContainerBuilder = DI.Models.ContainerBuilder;

namespace DI.Benchmarks;

[MemoryDiagnoser]
public class ContainerBenchmark
{
    private readonly IScope _reflectionBased, _lambdaBased;
    private readonly ILifetimeScope _scope;
    private readonly IServiceScope _provider;
    public ContainerBenchmark()
    {
      var lambdaBuilder =  new ContainerBuilder(new LambdaActivationBuild());
      var reflectionBuilder = new ContainerBuilder(new ReflectionBasedActivationBuilder());
      InitController(lambdaBuilder);
      InitController(reflectionBuilder);
      _lambdaBased = lambdaBuilder.Build().CreateScope();
      _reflectionBased = reflectionBuilder.Build().CreateScope();
      _scope = InitAutofac();
      _provider = InitMSDI();
    }
    
    
    [Benchmark(Baseline = true)]
    public Controller Create() => new Controller(new Service());
    
    [Benchmark(Description = "Reflection")]
    public Controller Reflection() => (Controller)_reflectionBased.Resolve(typeof(Controller));
    
    [Benchmark(Description = "Lambda")]
    public Controller Lambda() => (Controller)_lambdaBased.Resolve(typeof(Controller));


    [Benchmark(Description = "Autofac")]
    public Controller Autofac() => _scope.Resolve<Controller>();
    
    [Benchmark(Description = "MSDI")]
    public Controller MSDI() => _provider.ServiceProvider.GetRequiredService<Controller>();


    private ILifetimeScope InitAutofac()
    {
       var containerBuilder =  new Autofac.ContainerBuilder();
       containerBuilder.RegisterType<Service>().As<IService>();
       containerBuilder.RegisterType<Controller>().AsSelf();
       return containerBuilder.Build().BeginLifetimeScope();
    }

    private IServiceScope InitMSDI()
    {
        var collection = new ServiceCollection();
        collection.AddTransient<IService, Service>();
        collection.AddTransient<Controller, Controller>();
        return collection.BuildServiceProvider().CreateScope();
    }
    
    
    private void InitController(ContainerBuilder builder)
    {
        builder.RegisterTransient<IService, Service>()
            .RegisterTransient<Controller, Controller>();
    }
}