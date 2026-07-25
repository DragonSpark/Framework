using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Composition;

sealed class IsNativeFrameworkType : Condition<Type>
{
	public static IsNativeFrameworkType Default { get; } = new();

	IsNativeFrameworkType() : base(x => x.Namespace?.StartsWith("Microsoft.") ?? false) {}
}