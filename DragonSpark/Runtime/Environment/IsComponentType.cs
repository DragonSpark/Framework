using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Activation;
using System;

namespace DragonSpark.Runtime.Environment;

sealed class IsComponentType : AllCondition<Type>
{
	public static IsComponentType Default { get; } = new IsComponentType();

	IsComponentType() : base(CanConstruct.Default, CanActivate.Default,
	                         Is.DecoratedWith<InfrastructureAttribute>()
	                           .Then()
	                           .Inverse()
	                           .Get()) {}
}