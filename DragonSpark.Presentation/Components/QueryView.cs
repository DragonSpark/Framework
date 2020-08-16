using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components
{
	public class QueryView<T> : ISelect<LoadDataArgs, Task>
	{
		readonly IQueryable<T> _source;

		public QueryView(IQueryable<T> source) => _source = source;

		public int Count { get; private set; }

		public IEnumerable<T> Current { get; private set; } = default!;

		public async Task Get(LoadDataArgs load)
		{
			var ordered = !string.IsNullOrEmpty(load.OrderBy) ? _source.OrderBy(load.OrderBy) : _source;
			var all     = !string.IsNullOrEmpty(load.Filter) ? ordered.Where(load.Filter) : ordered;
			Count   = await all.CountAsync();
			Current = await all.Skip(load.Skip.GetValueOrDefault()).Take(load.Top.GetValueOrDefault()).ToArrayAsync();
		}
	}
}