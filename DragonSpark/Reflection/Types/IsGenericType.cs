using DragonSpark.Model.Selection.Conditions;
using System.Reflection;

namespace DragonSpark.Reflection.Types;

sealed class IsGenericType : Condition<TypeInfo>
{
	public static IsGenericType Default { get; } = new IsGenericType();

	IsGenericType() : base(x => x.IsGenericType) {}
}