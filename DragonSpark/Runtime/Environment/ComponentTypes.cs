using System;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Runtime.Environment
{
	sealed class ComponentTypes : Assume<Type, Array<Type>>
	{
		public static ComponentTypes Default { get; } = new ComponentTypes();

		ComponentTypes() : base(A.This(ComponentTypesDefinition.Default)
		                         .Select(x => x.ToStore())
		                         .ToContextual()
		                         .AsDefined()
		                         .Then()
		                         .Delegate()
		                         .Selector()) {}
	}
}