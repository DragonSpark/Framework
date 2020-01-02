using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Activation;
using System;

namespace DragonSpark.Runtime.Environment
{
	sealed class IsComponentTypeCandidate : AllCondition<Type>
	{
		public static IsComponentTypeCandidate Default { get; } = new IsComponentTypeCandidate();

		IsComponentTypeCandidate() : base(CanConstruct.Default, CanActivate.Default,
		                                  Is.DecoratedWith<InfrastructureAttribute>()
		                                    .Then()
		                                    .Inverse()
		                                    .Get()) {}
	}
}