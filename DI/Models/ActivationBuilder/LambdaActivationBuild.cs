using System.Linq.Expressions;
using System.Reflection;
using DI.Descriptors;
using DI.Interfaces;

namespace DI.Models.ActivationBuilder;

public class LambdaActivationBuild : BaseActivationBuilder
{
    private static readonly MethodInfo ResolveMethod = typeof(IScope).GetMethod("Resolve");
    
    protected override Func<IScope, object> BuildActivationInternal(
        TypeBasedServiceDescriptor typeBasedServiceDescriptor, ConstructorInfo constructor,
        ParameterInfo[] parameters, ServiceDescriptor descriptor)
    {
        var parameterExpression = Expression.Parameter(typeof(IScope), "scope");
        var constructorArgs = parameters.Select(x =>
            Expression.Convert(Expression.Call(parameterExpression, ResolveMethod, Expression.Constant(x.ParameterType)), x.ParameterType));
        var @new = Expression.New(constructor, constructorArgs);
        var lambda = Expression.Lambda<Func<IScope, object>>(@new, parameterExpression);
        return lambda.Compile();
    }
}