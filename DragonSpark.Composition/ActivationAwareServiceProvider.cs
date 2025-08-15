using System;
using System.Threading.Tasks;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Activation;
using JetBrains.Annotations;

namespace DragonSpark.Composition;

[MustDisposeResource]
sealed class ActivationAwareServiceProvider : IServiceProvider, IDisposable, IAsyncDisposable
{
    readonly IActivator _activator;
    readonly ICondition<Type> _condition;
    readonly IServiceProvider _provider;

    [MustDisposeResource(false)]
    public ActivationAwareServiceProvider(IServiceProvider provider) : this(provider, CanActivate.Default) {}

    [MustDisposeResource(false)]
    public ActivationAwareServiceProvider(IServiceProvider provider, ICondition<Type> condition)
        : this(provider, condition, Runtime.Activation.Activator.Default) { }

    public ActivationAwareServiceProvider(IServiceProvider provider, ICondition<Type> condition, IActivator activator)
    {
        _provider = provider;
        _condition = condition;
        _activator = activator;
    }

    public object? GetService(Type serviceType)
    {
        try
        {
            return _provider.GetService(serviceType);
        }
        catch (InvalidOperationException) when (_condition.Get(serviceType))
        {
            return _activator.Get(serviceType);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"A problem was encountered while resolving type {serviceType}", e);
        }
    }

    public void Dispose()
    {
        DisposeAny.Default.Execute(_provider);
    }

    public ValueTask DisposeAsync() => DisposingAny.Default.Get(_provider);
}
