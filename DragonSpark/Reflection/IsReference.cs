using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Reflection.Types;
using System.Reflection;

namespace DragonSpark.Reflection
{
	sealed class IsReference : InverseCondition<TypeInfo>
	{
		public static IsReference Default { get; } = new IsReference();

		IsReference() : base(IsValueType.Default) {}
	}
}