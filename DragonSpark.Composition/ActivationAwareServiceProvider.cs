using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Activation;
using JetBrains.Annotations;
using LightInject;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition;

[MustDisposeResource]
sealed class ActivationAwareServiceProvider : IKeyedServiceProvider, IDisposable, IAsyncDisposable
{
	readonly IActivator            _activator;
	readonly ICondition<Type>      _condition;
	readonly IKeyedServiceProvider _provider;

	[MustDisposeResource(false)]
	public ActivationAwareServiceProvider(IServiceContainer container, IServiceProvider provider) 
		: this(new KeyedServiceProvider(container, provider)) {}

	[MustDisposeResource(false)]
	public ActivationAwareServiceProvider(IKeyedServiceProvider provider) : this(provider, CanActivate.Default) {}

	[MustDisposeResource(false)]
	public ActivationAwareServiceProvider(IServiceContainer container, IServiceProvider provider,
	                                      ICondition<Type> condition)
		: this(new KeyedServiceProvider(container, provider), condition) {}

	[MustDisposeResource(false)]
	public ActivationAwareServiceProvider(IKeyedServiceProvider provider, ICondition<Type> condition)
		: this(provider, condition, Runtime.Activation.Activator.Default) {}

	public ActivationAwareServiceProvider(IKeyedServiceProvider provider, ICondition<Type> condition,
	                                      IActivator activator)
	{
		_provider  = provider;
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

	public object? GetKeyedService(Type serviceType, object? serviceKey)
		=> _provider.GetKeyedService(serviceType, serviceKey);

	public object GetRequiredKeyedService(Type serviceType, object? serviceKey)
		=> _provider.GetRequiredKeyedService(serviceType, serviceKey);

	public void Dispose()
	{
		DisposeAny.Default.Execute(_provider);
	}

	public ValueTask DisposeAsync() => DisposingAny.Default.Get(_provider);
}