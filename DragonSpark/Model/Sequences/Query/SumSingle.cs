using System;

namespace DragonSpark.Model.Sequences.Query
{
	public sealed class SumSingle : SumSingle<float>
	{
		public static SumSingle Default { get; } = new SumSingle();

		SumSingle() : base(x => x) {}
	}

	public class SumSingle<T> : Unlimited, IReduce<T, float>
	{
		readonly Func<T, float> _project;

		public SumSingle(Func<T, float> project) => _project = project;

		public float Get(Store<T> parameter)
		{
			var to     = parameter.Length;
			var array  = parameter.Instance;
			var result = 0f;
			for (var i = 0u; i < to; i++)
			{
				result += _project(array[i]);
			}

			return result;
		}
	}
}