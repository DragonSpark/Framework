using System;

namespace DragonSpark.Model.Sequences.Query.Construction
{
	sealed class LinkedContainer<TIn, TOut, TTo> : IContentContainer<TIn, TTo>
	{
		readonly Func<Assigned<uint>, IContent<TIn, TOut>> _from;
		readonly IContentContainer<TOut, TTo>              _to;

		public LinkedContainer(Func<Assigned<uint>, IContent<TIn, TOut>> from, IContentContainer<TOut, TTo> to)
		{
			_from = from;
			_to   = to;
		}

		public Func<Assigned<uint>, IContent<TIn, TTo>> Get(IStores<TTo> parameter)
			=> new Next<TIn, TOut, TTo>(_from, _to.Get(parameter)).Get;
	}
}