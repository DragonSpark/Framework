using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace DragonSpark.Application.Model
{
	public class Remove<TFrom, TTo> : Select<TFrom, TTo>
	{
		public Remove(ISelect<TFrom, TTo> previous, IMemoryCache memory, Func<TFrom, string> key)
			: this(previous, memory.Remove, key) {}

		public Remove(ISelect<TFrom, TTo> previous, Action<string> remove, Func<TFrom, string> key)
			: base(Start.A.Selection(key)
			            .Then()
			            .Terminate(remove)
			            .ToConfiguration()
			            .Select(previous)) {}

		public Remove(ISelect<TFrom, TTo> previous, Func<string, bool> remove, Func<TFrom, string> key)
			: base(Start.A.Selection(key)
			            .Then()
			            .Terminate(remove)
			            .ToConfiguration()
			            .Select(previous)) {}
	}
}