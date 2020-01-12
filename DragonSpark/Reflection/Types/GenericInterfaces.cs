using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Model.Sequences;
using NetFabric.Hyperlinq;
using System;

namespace DragonSpark.Reflection.Types
{
	sealed class GenericInterfaces : Store<Type, Array<Type>>
	{
		public static GenericInterfaces Default { get; } = new GenericInterfaces();

		GenericInterfaces() : base(AllInterfaces.Default.Then()
		                                        .Select(x => x.AsValueEnumerable()
		                                                      .Where(y => y.IsGenericType)
		                                                      .ToArray()
		                                                      .Result())) {}
	}
}