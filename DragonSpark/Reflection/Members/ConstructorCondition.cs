using System;
using System.Reflection;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Reflection.Members
{
	sealed class ConstructorCondition : Condition<ConstructorInfo>
	{
		public static ConstructorCondition Default { get; } = new ConstructorCondition();

		ConstructorCondition()
			: base(Start.An.Instance(Parameters.Default)
			            .Open()
			            .Then()
			            .To(x => x.HasNone()
			                      .Or(x.AllAre(y => y.IsOptional || y.Has<ParamArrayAttribute>())))) {}
	}
}