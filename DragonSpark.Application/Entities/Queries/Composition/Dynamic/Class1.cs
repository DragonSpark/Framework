using DragonSpark.Application.Entities.Queries.Materialize;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Composition.Dynamic
{
	class Class1 {}

/*    public IEnumerable<FilterDescriptor> Filters { get; set; }

	public IEnumerable<SortDescriptor> Sorts { get; set; }*/

	public class DynamicQueryInput
	{
		public bool IncludeTotalCount { get; set; }

		public string? OrderBy { get; set; }

		public string? Filter { get; set; }

		public QueryRange? Range { get; set; }

		/*public IEnumerable<FilterDescriptor> Filters { get; set; }

		public IEnumerable<SortDescriptor> Sorts { get; set; }*/
	}

	public interface ICompose<T> : ISelecting<ComposeInput<T>, Composition<T>> {}

	public readonly record struct ComposeInput<T>(DynamicQueryInput Input, IQueryable<T> Current);

	public readonly record struct Composition<T>(IQueryable<T> Current, ulong? Count = null);

	public readonly record struct QueryRange(int? Skip, int? Top);

	sealed class QueryViews<T> : ISelecting<DynamicQueryInput, QueryView<T>>
	{
		readonly IQueries<T> _source;
		readonly ICompose<T> _compose;
		readonly IToArray<T> _materialize;

		public QueryViews(IQueries<T> source, ICompose<T> compose, IToArray<T> materialize)
		{
			_source      = source;
			_compose     = compose;
			_materialize = materialize;
		}

		public async ValueTask<QueryView<T>> Get(DynamicQueryInput parameter)
		{
			using var session = await _source.Await();
			var (query, count) = await _compose.Await(new(parameter, session.Subject));
			var materialize = await _materialize.Await(query);
			return new(materialize.Open(), count);
		}
	}

	public class QueryView<T> : Collection<T>
	{
		public QueryView(IList<T> list, ulong? total) : base(list) => Total = total;

		public ulong? Total { get; }
	}

	/*public interface IQueryView<out T> : IAllocated<LoadDataArgs>
	{
		IEnumerable<T> Current { get; }

		ulong Count { get; }
	}*/
}