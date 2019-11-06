using System.Reflection;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Reflection.Types
{
	sealed class IsValueType : ICondition<TypeInfo>
	{
		public static IsValueType Default { get; } = new IsValueType();

		IsValueType() {}

		public bool Get(TypeInfo parameter) => parameter.IsValueType;
	}
}