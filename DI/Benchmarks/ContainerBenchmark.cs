using BenchmarkDotNet.Attributes;
using DI.Controllers;
using DI.Extensions;
using DI.Interfaces;
using DI.Interfaces.Services;
using DI.Models;
using DI.Models.ActivationBuilder;
using DI.Services;

namespace DI.Benchmarks;

public class ContainerBenchmark
{
    private readonly IScope _reflectionBased, _lambdaBased;
    public ContainerBenchmark()
    {
      var lambdaBuilder =  new ContainerBuilder(new LambdaActivationBuild());
      var reflectionBuilder = new ContainerBuilder(new ReflectionBasedActivationBuilder());
      InitController(lambdaBuilder);
      InitController(reflectionBuilder);
      _lambdaBased = lambdaBuilder.Build().CreateScope();
      _reflectionBased = reflectionBuilder.Build().CreateScope();
    }

    private void InitController(ContainerBuilder builder)
    {
        builder.RegisterTransient<IService, Service>()
            .RegisterTransient<Controller, Controller>();
    }
    
    [Benchmark(Baseline = true)]
    public Controller Create() => new Controller(new Service());
    
    [Benchmark(Description = "Reflection")]
    public Controller Reflection() => (Controller)_reflectionBased.Resolve(typeof(Controller));
    
    [Benchmark(Description = "Lambda")]
    public Controller Lambda() => (Controller)_lambdaBased.Resolve(typeof(Controller));
    
}