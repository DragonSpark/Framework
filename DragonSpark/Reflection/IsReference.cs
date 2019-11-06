using System.Reflection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Reflection.Types;

namespace DragonSpark.Reflection
{
	sealed class IsReference : InverseCondition<TypeInfo>
	{
		public static IsReference Default { get; } = new IsReference();

		IsReference() : base(IsValueType.Default) {}
	}
}