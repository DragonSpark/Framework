using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Selection;
using System;

namespace DragonSpark.Runtime.Environment
{
	sealed class ComponentTypesDefinition : Assume<Type, Array<Type>>
	{
		public static ComponentTypesDefinition Default { get; } = new ComponentTypesDefinition();

		ComponentTypesDefinition() : this(Types<PublicAssemblyTypes>.Default) {}

		public ComponentTypesDefinition(IArray<Type> types) : this(types, IsComponentTypeCandidate.Default.Get) {}

		ComponentTypesDefinition(IArray<Type> types, Func<Type, bool> condition)
			: base(types.Query()
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