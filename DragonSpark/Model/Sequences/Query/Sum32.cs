using System;

namespace DragonSpark.Model.Sequences.Query
{
	public sealed class Sum32 : Sum32<int>
	{
		public static Sum32 Default { get; } = new Sum32();

		Sum32() : base(i => i) {}
	}

	public class Sum32<T> : Unlimited, IReduce<T, int>
	{
		readonly Func<T, int> _project;

		public Sum32(Func<T, int> project) => _project = project;

		public int Get(Store<T> parameter)
		{
			var to     = parameter.Length;
			var array  = parameter.Instance;
			var result = 0;
			for (var i = 0u; i < to; i++)
			{
				result += _project(array[i]);
			}

			return result;
		}
	}
}