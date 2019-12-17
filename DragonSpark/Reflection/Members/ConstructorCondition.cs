using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members
{
	sealed class ConstructorCondition : Condition<ConstructorInfo>
	{
		public static ConstructorCondition Default { get; } = new ConstructorCondition();

		ConstructorCondition()
			: base(Start.A.Selection(Parameters.Default)
			            .Open()
			            .Then()
			            .To(x => x.HasNone()
			                      .Or(x.AllAre(y => y.IsOptional || y.Has<ParamArrayAttribute>())))) {}
	}
}