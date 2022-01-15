using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Activation;
using System;

namespace DragonSpark.Composition;

sealed class ActivationAwareServiceProvider : IServiceProvider, IDisposable
{
	readonly IActivator       _activator;
	readonly ICondition<Type> _condition;
	readonly IServiceProvider _provider;

	public ActivationAwareServiceProvider(IServiceProvider provider)
		: this(provider, CanActivate.Default, Runtime.Activation.Activator.Default) {}

	public ActivationAwareServiceProvider(IServiceProvider provider, ICondition<Type> condition,
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

	public void Dispose()
	{
		_provider.ToDisposable().Dispose();
	}
}