using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Compose.Store.Operations
{
	public class Removing<TFrom, TTo> : Remove<TFrom, ValueTask<TTo>>, ISelecting<TFrom, TTo>
	{
		public Removing(ISelect<TFrom, ValueTask<TTo>> previous, IMemoryCache memory, Func<TFrom, string> key)
			: base(previous, memory, key) {}

		public Removing(ISelect<TFrom, ValueTask<TTo>> previous, Action<string> remove, Func<TFrom, string> key)
			: base(previous, remove, key) {}

		public Removing(ISelect<TFrom, ValueTask<TTo>> previous, Func<string, bool> remove, Func<TFrom, string> key)
			: base(previous, remove, key) {}
	}
}