using System.Reflection;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Reflection.Types
{
	sealed class HasGenericArguments : AllCondition<TypeInfo>
	{
		public static HasGenericArguments Default { get; } = new HasGenericArguments();

		HasGenericArguments() : base(IsGenericType.Default.ToDelegate(),
		                             GenericArguments.Default.Open().Then().HasAny()) {}
	}
}