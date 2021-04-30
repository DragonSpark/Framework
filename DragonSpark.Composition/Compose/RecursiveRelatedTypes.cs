using DragonSpark.Model.Sequences;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;

namespace DragonSpark.Composition.Compose
{
	sealed class RecursiveRelatedTypes : IRelatedTypes
	{
		readonly IRelatedTypes _related;

		public RecursiveRelatedTypes(IRelatedTypes related) => _related = related;

		IEnumerable<Type> Yield(Type current)
		{
			foreach (var type in _related.Get(current).Open())
			{
				yield return type;

				foreach (var other in Yield(type).AsValueEnumerable())
				{
					yield return other!;
				}
			}
		}

		// TODO: Performance:
		public Array<Type> Get(Type parameter) => Yield(parameter).AsValueEnumerable().Distinct().ToArray();
	}
}