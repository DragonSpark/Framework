using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Model.Sequences;
using NetFabric.Hyperlinq;
using System;

namespace DragonSpark.Reflection.Types;

sealed class AllInterfaces : Store<Type, Array<Type>>
{
	public static AllInterfaces Default { get; } = new AllInterfaces();

	AllInterfaces() : base(TypeMetadata.Default.Then()
	                                   .Select(Interfaces.Default)
	                                   .Select(x => x.AsValueEnumerable()
	                                                 .Where(y => y.IsInterface)
	                                                 .Distinct()
	                                                 .ToArray()
	                                                 .Result())
	                      ) {}
}