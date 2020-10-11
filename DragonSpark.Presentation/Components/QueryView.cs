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

		public async Task Get(LoadDataArgs parameter)
		{
			var ordered = !string.IsNullOrEmpty(parameter.OrderBy) ? _source.OrderBy(parameter.OrderBy) : _source;
			var all     = !string.IsNullOrEmpty(parameter.Filter) ? ordered.Where(parameter.Filter) : ordered;
			Count   = await all.CountAsync();
			Current = await all.Skip(parameter.Skip.GetValueOrDefault()).Take(parameter.Top.GetValueOrDefault()).ToArrayAsync();
		}
	}
}