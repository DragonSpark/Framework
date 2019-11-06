using System;

namespace DragonSpark.Model.Sequences.Query
{
	public sealed class SumUnsigned64 : SumUnsigned64<ulong>
	{
		public static SumUnsigned64 Default { get; } = new SumUnsigned64();

		SumUnsigned64() : base(x => x) {}
	}

	public class SumUnsigned64<T> : Unlimited, IReduce<T, ulong>
	{
		readonly Func<T, ulong> _project;

		public SumUnsigned64(Func<T, ulong> project) => _project = project;

		public ulong Get(Store<T> parameter)
		{
			var to     = parameter.Length;
			var array  = parameter.Instance;
			var result = 0ul;
			for (var i = 0u; i < to; i++)
			{
				result += _project(array[i]);
			}

			return result;
		}
	}
}