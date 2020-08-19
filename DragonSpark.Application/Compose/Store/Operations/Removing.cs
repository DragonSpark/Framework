using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Compose.Store.Operations
{
	public class Removing<TFrom, TTo> : Remove<TFrom, ValueTask<TTo>>, ISelecting<TFrom, TTo>
	{
		public Removing([NotNull] ISelect<TFrom, ValueTask<TTo>> previous, [NotNull] IMemoryCache memory,
		                [NotNull] Func<TFrom, string> key) : base(previous, memory, key) {}
	}
}