using System;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Runtime.Environment
{
	sealed class ComponentTypesDefinition : Model.Results.Result<ISelect<Type, Array<Type>>>
	{
		public static ComponentTypesDefinition Default { get; } = new ComponentTypesDefinition();

		ComponentTypesDefinition() : this(Types.Default.Query()
		                                       .Where(CanActivate.Default.Get)
		                                       .To(x => x.Get().Then())
		                                       .Activate<ComponentTypesSelector>()
		                                       .Select(x => x.Open().Then().Sort().Out())
		                                       .Selector()) {}

		public ComponentTypesDefinition(Func<ISelect<Type, Array<Type>>> source) : base(source) {}
	}
}