using System.Reflection;
using DragonSpark.Model.Selection;

namespace DragonSpark.Reflection.Members
{
	public sealed class PropertyAccessMethodSelector : ISelect<PropertyInfo, MethodInfo>
	{
		public static PropertyAccessMethodSelector Default { get; } = new PropertyAccessMethodSelector();

		PropertyAccessMethodSelector() {}

		public MethodInfo Get(PropertyInfo parameter) => parameter.GetMethod;
	}
}