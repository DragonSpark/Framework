using System;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Runtime.Activation;

sealed class CanConstruct : ICondition<Type>
{
	public static CanConstruct Default { get; } = new();

	CanConstruct() {}

	public bool Get(Type parameter) => parameter is { IsAbstract: false, IsClass: true };
}