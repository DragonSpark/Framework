using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Composition.Compose
{
	sealed class RelatedTypes : FixedResult<Type, Array<Type>>, IRelatedTypes
	{
		public static RelatedTypes Default { get; } = new RelatedTypes();

		RelatedTypes() : base(Array<Type>.Empty) {}
	}
}