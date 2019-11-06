using System;

namespace DragonSpark.Model.Sequences.Query
{
	public sealed class Sum64 : Sum64<long>
	{
		public static Sum64 Default { get; } = new Sum64();

		Sum64() : base(x => x) {}
	}

	public class Sum64<T> : Unlimited, IReduce<T, long>
	{
		readonly Func<T, long> _project;

		public Sum64(Func<T, long> project) => _project = project;

		public long Get(Store<T> parameter)
		{
			var to     = parameter.Length;
			var array  = parameter.Instance;
			var result = 0L;
			for (var i = 0u; i < to; i++)
			{
				result += _project(array[i]);
			}

			return result;
		}
	}
}