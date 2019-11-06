using System.Reflection;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Runtime.Activation
{
	sealed class CanActivate : ICondition<TypeInfo>
	{
		readonly static TypeInfo GeneralObject = typeof(object).GetTypeInfo();

		public static CanActivate Default { get; } = new CanActivate();

		CanActivate() {}

		public bool Get(TypeInfo parameter)
			=> !parameter.IsAbstract && parameter.IsClass && !parameter.Equals(GeneralObject);
	}
}