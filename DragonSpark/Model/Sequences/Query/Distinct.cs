using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Query
{
	sealed class Distinct<T> : IBody<T>
	{
		readonly IEqualityComparer<T> _comparer;

		public Distinct(IEqualityComparer<T> comparer) => _comparer = comparer;

		public ArrayView<T> Get(ArrayView<T> parameter)
		{
			var from  = parameter.Start;
			var to    = from + parameter.Length;
			var array = parameter.Array;
			var count = 0u;
			var set   = new Set<T>(_comparer);
			for (var i = from; i < to; i++)
			{
				var item = array[i];
				if (set.Add(in item))
				{
					array[count++] = item;
				}
			}

			set.Clear();

			return new ArrayView<T>(array, 0, count);
		}
	}
}