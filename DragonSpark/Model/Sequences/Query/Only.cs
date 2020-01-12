using DragonSpark.Model.Selection;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Query
{
	/*public sealed class Only<T> : One<T>
	{
		public static Only<T> Default { get; } = new Only<T>();

		Only() : this(Always<T>.Default.Get) {}

		public Only(Func<T, bool> where) : base(where) {}
	}*/

	sealed class Only<T> : ISelect<IEnumerable<T>, T>
	{
		public static Only<T> Default { get; } = new Only<T>();

		Only() {}

		public T Get(IEnumerable<T> parameter)
		{
			using var enumerator = parameter.GetEnumerator();
			var first = enumerator.MoveNext() ? enumerator.Current : default;
			var result = !enumerator.MoveNext() ? first : default;
			return result;
		}
	}
}