using System.Collections.Generic;
using System.Reflection;
using DragonSpark.Reflection.Types;

namespace DragonSpark.Reflection.Collections
{
	public sealed class ImplementsGenericEnumerable : ImplementsGenericType
	{
		public static ImplementsGenericEnumerable Default { get; } = new ImplementsGenericEnumerable();

		ImplementsGenericEnumerable() : base(typeof(IEnumerable<>).GetTypeInfo()) {}
	}
}