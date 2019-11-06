using System;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Query
{
	public class Union<T> : IContent<T, T>
	{
		readonly IBody<T>             _body;
		readonly IEqualityComparer<T> _comparer;
		readonly Assigned<uint>       _limit;
		readonly ISequence<T>         _others;
		readonly IStores<T>           _stores;

		// ReSharper disable once TooManyDependencies
		public Union(IBody<T> body, ISequence<T> others, IEqualityComparer<T> comparer, IStores<T> stores,
		             Assigned<uint> limit)
		{
			_body     = body;
			_others   = others;
			_comparer = comparer;
			_stores   = stores;
			_limit    = limit;
		}

		public Store<T> Get(Store<T> parameter)
		{
			var other     = _others.Get();
			var source    = other.Instance;
			var appending = other.Length;

			var body   = _body.Get(new ArrayView<T>(parameter.Instance, 0, parameter.Length));
			var @in    = body.Array;
			var length = body.Length;

			var set = new Set<T>(_comparer);
			for (var i = body.Start; i < body.Start + length; i++)
			{
				set.Add(in @in[i]);
			}

			var count = 0u;
			for (var i = 0u; i < appending; i++)
			{
				var item = source[i];
				if (set.Add(in item))
				{
					source[count++] = item;
				}
			}

			var result = _stores.Get(_limit.IsAssigned
				                         ? Math.Min(_limit.Instance, length + count)
				                         : length + count);

			var @out = @in.CopyInto(result.Instance, body.Start, body.Length);
			for (var i = 0; i < count; i++)
			{
				@out[i + length] = source[i];
			}

			return result;
		}
	}
}