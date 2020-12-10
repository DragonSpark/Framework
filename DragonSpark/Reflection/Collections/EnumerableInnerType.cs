using DragonSpark.Model.Selection;
using System.Reflection;

namespace DragonSpark.Reflection.Collections
{
	public sealed class EnumerableInnerType : Select<TypeInfo, TypeInfo?>
	{
		public static EnumerableInnerType Default { get; } = new EnumerableInnerType();

		EnumerableInnerType() : base(new InnerType(ImplementsGenericEnumerable.Default)) {}
	}
}