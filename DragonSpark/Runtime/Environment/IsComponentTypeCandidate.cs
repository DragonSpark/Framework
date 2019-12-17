using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Reflection;
using DragonSpark.Runtime.Activation;
using System;

namespace DragonSpark.Runtime.Environment
{
	sealed class IsComponentTypeCandidate : AllCondition<Type>
	{
		public static IsComponentTypeCandidate Default { get; } = new IsComponentTypeCandidate();

		IsComponentTypeCandidate() : base(CanConstruct.Default, CanActivate.Default,
		                                  IsDecoratedWith<InfrastructureAttribute>.Default.Then().Inverse().Get()) {}
	}

	public sealed class InfrastructureAttribute : Attribute {}
}