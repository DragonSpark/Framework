using DragonSpark.Model.Selection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Composition;

sealed class LocateAwareServiceProvider : IServiceProvider
{
	readonly IServiceProvider     _previous;
	readonly ISelect<Type, Type?> _locate;

	public LocateAwareServiceProvider(IServiceProvider previous, IServiceCollection collection)
		: this(previous, new LocateImplementation(collection)) {}

	public LocateAwareServiceProvider(IServiceProvider previous, ISelect<Type, Type?> locate)
	{
		_previous = previous;
		_locate   = locate;
	}

	public object? GetService(Type serviceType)
	{
		try
		{
			return _previous.GetService(serviceType);
		}
		catch (InvalidOperationException)
		{
			var located = _locate.Get(serviceType);
			if (located != null)
			{
				return _previous.GetService(located);
			}

			throw;
		}
	}
}