using DragonSpark.Model.Selection;
using Microsoft.Extensions.DependencyInjection;
using NetFabric.Hyperlinq;
using System;

namespace DragonSpark.Composition
{
	sealed class LocateImplementation : ISelect<Type, Type?>
	{
		readonly IServiceCollection _collection;

		public LocateImplementation(IServiceCollection collection) => _collection = collection;

		public Type? Get(Type parameter)
		{
			foreach (var element in _collection.AsValueEnumerable())
			{
				var descriptor = element!;

				if (descriptor.ServiceType == parameter)
				{
					if (descriptor.ImplementationType != null)
					{
						return descriptor.ImplementationType;
					}

					break;
				}
			}

			return null;
		}
	}
}