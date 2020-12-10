using DragonSpark.Model.Selection;
using System.Reflection;

namespace DragonSpark.Reflection.Collections
{
	public sealed class CollectionInnerType : Select<TypeInfo, TypeInfo?>
	{
		public static CollectionInnerType Default { get; } = new CollectionInnerType();

		CollectionInnerType() : base(new InnerType(ImplementsGenericCollection.Default)) {}
	}
}