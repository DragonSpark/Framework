using DragonSpark.Runtime;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;

namespace DragonSpark.Composition.Compose
{
	sealed class RecursiveRelatedTypes : IRelatedTypes
	{
		readonly IRelatedTypes _related;

		public RecursiveRelatedTypes(IRelatedTypes related) => _related = related;

		void Yield(List<Type> all, Type current)
		{
			var related = _related.Get(current);
			all.AddRange(related);
			foreach (var type in related)
			{
				Yield(all, type);
			}
		}

		public List<Type> Get(Type parameter)
		{
			var result = new List<Type>();
			Yield(result, parameter);
			return result;
		}
	}
}