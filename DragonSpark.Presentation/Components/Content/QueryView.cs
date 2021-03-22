using Radzen;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	sealed class QueryView<T> : IQueryView<T>
	{
		readonly IQueryable<T>       _source;
		readonly ApplicationQuery<T> _query;
		readonly IQueryAlteration<T> _alteration;

		public QueryView(IQueryable<T> source, ApplicationQuery<T> query)
			: this(source, query, DefaultQueryAlteration<T>.Default) {}

		public QueryView(IQueryable<T> source, ApplicationQuery<T> query, IQueryAlteration<T> alteration)
		{
			_source     = source;
			_query      = query;
			_alteration = alteration;
		}

		public ulong Count { get; private set; }

		public IEnumerable<T> Current { get; private set; } = default!;

		public async Task Get(LoadDataArgs parameter)
		{
			var all = _alteration.Get(new(_source, parameter));
			Count = (ulong)await _query.Count.Long.Get(all);
			Current = await _query.ToArray.Get(all.Skip(parameter.Skip.GetValueOrDefault())
			                                      .Take(parameter.Top.GetValueOrDefault())
			                                  );
		}
	}
}