using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Runtime.Environment
{
	sealed class ComponentTypesDefinition : Assume<Type, Array<Type>>
	{
		public static ComponentTypesDefinition Default { get; } = new ComponentTypesDefinition();

		ComponentTypesDefinition() : this(IsComponentTypeCandidate.Default.Get) {}

		ComponentTypesDefinition(Func<Type, bool> condition)
			: base(Start.An.Instance(Types.Default)
			            .Query()
			            .Where(condition)
			            .Get()
			            .Then()
			            .Activate<ComponentTypesSelector>()
			            .Select(x => x.Open()
			                          .Then()
			                          .Sort()
			                          .Out()
			                          .ToDelegate()
			                          )
			            .Selector()
			      ) {}
	}
}