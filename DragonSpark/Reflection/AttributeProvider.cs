using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System.Reflection;

namespace DragonSpark.Reflection
{
	sealed class AttributeProvider<T> : Select<T, ICustomAttributeProvider>
	{
		public static AttributeProvider<T> Default { get; } = new AttributeProvider<T>();

		AttributeProvider() : base(Start.A.Selection<T>()
		                                .By.Metadata
		                                .Cast<ICustomAttributeProvider>()
		                                .Unless.Input.IsOf(A.Self<ICustomAttributeProvider>())) {}
	}
}