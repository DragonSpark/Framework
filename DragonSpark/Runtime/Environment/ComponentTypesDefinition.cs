using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Runtime.Environment
{
	sealed class ComponentTypesDefinition : Model.Results.Result<ISelect<Type, Array<Type>>>
	{
		public static ComponentTypesDefinition Default { get; } = new ComponentTypesDefinition();

		ComponentTypesDefinition() : this(IsComponentTypeCandidate.Default.Get) {}

		ComponentTypesDefinition(Func<Type, bool> condition)
			: this(Types.Default.Query()
			            .Where(condition)
			            .To(x => x.Get().Then())
			            .Activate<ComponentTypesSelector>()
			            .Select(x => x.Open().Then().Sort().Out())
			            .Selector()) {}

		public ComponentTypesDefinition(Func<ISelect<Type, Array<Type>>> source) : base(source) {}
	}
}