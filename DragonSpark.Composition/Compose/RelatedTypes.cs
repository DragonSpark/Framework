using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;
using System.Collections.Generic;

namespace DragonSpark.Composition.Compose
{
	sealed class RelatedTypes : Select<Type, List<Type>>, IRelatedTypes
	{
		public static RelatedTypes Default { get; } = new RelatedTypes();

		RelatedTypes() : base(new List<Type>().Accept) {}
	}
}