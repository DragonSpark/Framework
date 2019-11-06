using System;

namespace DragonSpark.Model.Sequences.Query
{
	public sealed class SumDecimal : SumDecimal<decimal>
	{
		public static SumDecimal Default { get; } = new SumDecimal();

		SumDecimal() : base(x => x) {}
	}

	public class SumDecimal<T> : Unlimited, IReduce<T, decimal>
	{
		readonly Func<T, decimal> _project;

		public SumDecimal(Func<T, decimal> project) => _project = project;

		public decimal Get(Store<T> parameter)
		{
			var to     = parameter.Length;
			var array  = parameter.Instance;
			var result = 0m;
			for (var i = 0u; i < to; i++)
			{
				result += _project(array[i]);
			}

			return result;
		}
	}
}