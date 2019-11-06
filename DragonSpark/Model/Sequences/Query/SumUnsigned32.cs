using System;

namespace DragonSpark.Model.Sequences.Query
{
	public sealed class SumUnsigned32 : SumUnsigned32<uint>
	{
		public static SumUnsigned32 Default { get; } = new SumUnsigned32();

		SumUnsigned32() : base(x => x) {}
	}

	public class SumUnsigned32<T> : Unlimited, IReduce<T, uint>
	{
		readonly Func<T, uint> _project;

		public SumUnsigned32(Func<T, uint> project) => _project = project;

		public uint Get(Store<T> parameter)
		{
			var to     = parameter.Length;
			var array  = parameter.Instance;
			var result = 0u;
			for (var i = 0u; i < to; i++)
			{
				result += _project(array[i]);
			}

			return result;
		}
	}
}