using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Reflection.Types;

sealed class IsConstructedGenericType : Condition<Type>
{
	public static IsConstructedGenericType Default { get; } = new();

	IsConstructedGenericType() : base(x => x.IsConstructedGenericType) {}
}