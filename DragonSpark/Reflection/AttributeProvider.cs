using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System.Reflection;

namespace DragonSpark.Reflection
{
	sealed class AttributeProvider<T> : Select<T, ICustomAttributeProvider>
	{
		public static AttributeProvider<T> Default { get; } = new AttributeProvider<T>();

		AttributeProvider() : base(Start.A.Selection<T>()
		                                .By.Metadata.Then()
		                                .Cast<ICustomAttributeProvider>()
		                                .Or.Use(A.Self<ICustomAttributeProvider>())) {}
	}
}