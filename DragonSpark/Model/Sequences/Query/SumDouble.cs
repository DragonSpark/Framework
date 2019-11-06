using System;

namespace DragonSpark.Model.Sequences.Query
{
	public sealed class SumDouble : SumDouble<double>
	{
		public static SumDouble Default { get; } = new SumDouble();

		SumDouble() : base(x => x) {}
	}

	public class SumDouble<T> : Unlimited, IReduce<T, double>
	{
		readonly Func<T, double> _project;

		public SumDouble(Func<T, double> project) => _project = project;

		public double Get(Store<T> parameter)
		{
			var to     = parameter.Length;
			var array  = parameter.Instance;
			var result = 0d;
			for (var i = 0u; i < to; i++)
			{
				result += _project(array[i]);
			}

			return result;
		}
	}
}