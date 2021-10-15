using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Runtime.Activation;

sealed class CanConstruct : ICondition<Type>
{
	public static CanConstruct Default { get; } = new CanConstruct();

	CanConstruct() {}

	public bool Get(Type parameter) => !parameter.IsAbstract && parameter.IsClass;
}