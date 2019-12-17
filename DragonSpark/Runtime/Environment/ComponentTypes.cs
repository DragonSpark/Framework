using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Runtime.Environment
{
	sealed class ComponentTypes : Assume<Type, Array<Type>>
	{
		public static ComponentTypes Default { get; } = new ComponentTypes();

		ComponentTypes() : base(Start.An.Instance(ComponentTypesDefinition.Default)
		                             .Select(x => x.ToStore())
		                             .ToContextual()
		                             .AsDefined()
		                             .Then()
		                             .Delegate()
		                             .Selector()) {}
	}
}