using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Reflection.Types;

public sealed class IsNativeSystemType : Condition<Type>
{
	public static IsNativeSystemType Default { get; } = new();

	IsNativeSystemType() : base(x =>
	                            {
		                            var name = x.Module.ScopeName;
		                            return name.StartsWith("System.Private") || name.StartsWith("Microsoft.");
	                            }) {}
}