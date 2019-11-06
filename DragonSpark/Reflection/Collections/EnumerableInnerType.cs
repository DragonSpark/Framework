using System.Reflection;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Reflection.Collections
{
	public sealed class EnumerableInnerType : Alteration<TypeInfo>
	{
		public static EnumerableInnerType Default { get; } = new EnumerableInnerType();

		EnumerableInnerType() : base(new InnerType(ImplementsGenericEnumerable.Default)) {}
	}
}