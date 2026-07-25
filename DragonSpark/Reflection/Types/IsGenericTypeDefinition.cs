using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Reflection.Types;

sealed class IsGenericTypeDefinition : Condition<Type>
{
	public static IsGenericTypeDefinition Default { get; } = new();

	IsGenericTypeDefinition() : base(x => x.IsGenericTypeDefinition) {}
}