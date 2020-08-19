using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace DragonSpark.Application.Compose.Store
{
	public class Remove<TFrom, TTo> : ISelect<TFrom, TTo>
	{
		readonly ISelect<TFrom, TTo> _previous;
		readonly IMemoryCache        _memory;
		readonly Func<TFrom, string> _key;

		public Remove(ISelect<TFrom, TTo> previous, IMemoryCache memory, Func<TFrom, string> key)
		{
			_previous = previous;
			_memory   = memory;
			_key      = key;
		}

		public TTo Get(TFrom parameter)
		{
			_memory.Remove(_key(parameter));
			return _previous.Get(parameter);
		}
	}
}