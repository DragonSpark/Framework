using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using System.Reflection;

namespace DragonSpark.Reflection.Types
{
	sealed class HasGenericArguments : AllCondition<TypeInfo>
	{
		public static HasGenericArguments Default { get; } = new HasGenericArguments();

		HasGenericArguments() : base(IsGenericType.Default.Get, GenericArguments.Default.Then().HasAny()) {}
	}
}