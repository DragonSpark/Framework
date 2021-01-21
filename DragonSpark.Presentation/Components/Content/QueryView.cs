using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	public interface IQueryView<out T> : IAllocated<LoadDataArgs>
	{
		IEnumerable<T> Current { get; }

		ulong Count { get; }
	}

	sealed class QueryView<T> : IQueryView<T>
	{
		readonly IQueryable<T> _source;

		public QueryView(IQueryable<T> source) => _source = source;

		public ulong Count { get; private set; }

		public IEnumerable<T> Current { get; private set; } = default!;

		public async Task Get(LoadDataArgs parameter)
		{
			var ordered = !string.IsNullOrEmpty(parameter.OrderBy) ? _source.OrderBy(parameter.OrderBy) : _source;
			var all     = !string.IsNullOrEmpty(parameter.Filter) ? ordered.Where(parameter.Filter) : ordered;
			Count = (ulong)await all.LongCountAsync();
			Current = await all.Skip(parameter.Skip.GetValueOrDefault())
			                   .Take(parameter.Top.GetValueOrDefault())
			                   .ToArrayAsync();
		}
	}
}