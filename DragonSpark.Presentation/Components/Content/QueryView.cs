using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	sealed class QueryView<T> : IQueryView<T>
	{
		readonly IQueryable<T>       _source;
		readonly IQueryAlteration<T> _alteration;

		public QueryView(IQueryable<T> source) : this(source, DefaultQueryAlteration<T>.Default) {}

		public QueryView(IQueryable<T> source, IQueryAlteration<T> alteration)
		{
			_source     = source;
			_alteration = alteration;
		}

		public ulong Count { get; private set; }

		public IEnumerable<T> Current { get; private set; } = default!;

		public async Task Get(LoadDataArgs parameter)
		{
			var all = _alteration.Get(new(_source, parameter));
			Count = (ulong)await all.LongCountAsync();
			Current = await all.Skip(parameter.Skip.GetValueOrDefault())
			                   .Take(parameter.Top.GetValueOrDefault())
			                   .ToArrayAsync();
		}
	}
}