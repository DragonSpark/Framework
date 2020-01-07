using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using System;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Reflection.Members
{
	sealed class RemainingParametersAreOptional : ICondition<Array<ParameterInfo>>
	{
		public static RemainingParametersAreOptional Default { get; } =
			new RemainingParametersAreOptional();

		RemainingParametersAreOptional() {}

		public bool Get(Array<ParameterInfo> parameter)
			=> parameter.Open().Skip(1).All(x => x.IsOptional || x.Has<ParamArrayAttribute>());
	}
}