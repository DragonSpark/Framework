using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.TypeSystem
{
	public static class Items<T>
	{
		public static T[] Default { get; } = (T[])Enumerable.Empty<T>();

		public static ImmutableArray<T> Immutable { get; } = Default.ToImmutableArray();

		public static IList<T> List { get; } = Default.ToImmutableList();
	}
}