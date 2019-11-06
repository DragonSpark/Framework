using System.Reflection;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Reflection.Types
{
	sealed class IsGenericType : Condition<TypeInfo>
	{
		public static IsGenericType Default { get; } = new IsGenericType();

		IsGenericType() : base(x => x.IsGenericType) {}
	}
}