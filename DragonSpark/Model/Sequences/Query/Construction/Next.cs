using System;

namespace DragonSpark.Model.Sequences.Query.Construction
{
	sealed class Next<TIn, TOut, TTo> : ISelectedContent<TIn, TTo>
	{
		readonly Func<Assigned<uint>, IContent<TIn, TOut>> _from;
		readonly Func<Assigned<uint>, IContent<TOut, TTo>> _to;

		public Next(Func<Assigned<uint>, IContent<TIn, TOut>> from, Func<Assigned<uint>, IContent<TOut, TTo>> to)
		{
			_from = from;
			_to   = to;
		}

		public IContent<TIn, TTo> Get(Assigned<uint> parameter) => new Content(_from(parameter), _to(parameter));

		sealed class Content : IContent<TIn, TTo>
		{
			readonly IContent<TOut, TTo> _current;
			readonly IContent<TIn, TOut> _previous;

			public Content(IContent<TIn, TOut> previous, IContent<TOut, TTo> current)
			{
				_previous = previous;
				_current  = current;
			}

			public Store<TTo> Get(Store<TIn> parameter) => _current.Get(_previous.Get(parameter));
		}
	}
}