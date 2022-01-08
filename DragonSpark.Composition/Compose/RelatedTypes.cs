using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Memory;
using System;

namespace DragonSpark.Composition.Compose;

sealed class RelatedTypes : Select<Type, Leasing<Type>>, IRelatedTypes
{
	public static RelatedTypes Default { get; } = new();

	RelatedTypes() : base(_ => Leasing<Type>.Default) {}
}