using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Memory;
using System;

namespace DragonSpark.Composition.Compose
{
	sealed class RelatedTypes : Select<Type, Lease<Type>>, IRelatedTypes
	{
		public static RelatedTypes Default { get; } = new RelatedTypes();

		RelatedTypes() : base(_ => Lease<Type>.Default) {}
	}
}