using DragonSpark.Compose;
using DragonSpark.Reflection.Members;
using System;
using System.Reflection;

namespace DragonSpark.Runtime.Activation;

sealed class HasActivationConstructor : IsAssigned<Type, ConstructorInfo>
{
	public static HasActivationConstructor Default { get; } = new HasActivationConstructor();

	HasActivationConstructor() : base(ConstructorLocator.Default.Get) {}
}