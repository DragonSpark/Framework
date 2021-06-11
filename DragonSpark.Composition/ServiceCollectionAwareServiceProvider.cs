using Microsoft.Extensions.DependencyInjection;
using NetFabric.Hyperlinq;
using System;

namespace DragonSpark.Composition
{
	sealed class ServiceCollectionAwareServiceProvider : IServiceProvider
	{
		readonly IServiceProvider   _previous;
		readonly IServiceCollection _collection;

		public ServiceCollectionAwareServiceProvider(IServiceProvider previous, IServiceCollection collection)
		{
			_previous   = previous;
			_collection = collection;
		}

		public object? GetService(Type serviceType)
		{
			try
			{
				return _previous.GetService(serviceType);
			}
			catch (InvalidOperationException)
			{
				foreach (var element in _collection.AsValueEnumerable())
				{
					var descriptor = element!;

					if (descriptor.ServiceType == serviceType)
					{
						if (descriptor.ImplementationType != null)
						{
							return _previous.GetService(descriptor.ImplementationType);
						}

						break;
					}
				}

				throw;
			}
		}
	}
}