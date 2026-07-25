using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Selection;
using DragonSpark.Presentation.Components.Content.Sequences;
using Syncfusion.Blazor;

namespace DragonSpark.SyncfusionRendering.Queries.Data;

sealed class DataManagerRequests : ISelect<PageRequest, DataManagerRequest>
{
	public static DataManagerRequests Default { get; } = new();

	DataManagerRequests()
		: this(ComposeSearchFilterModel.Default.Get, ComposeWhereFilterModel.Default.Get, ComposeSortModel.Default.Get) {}

	readonly Func<SearchFilter, Syncfusion.Blazor.Data.SearchFilter> _search;
	readonly Func<WhereFilter, Syncfusion.Blazor.Data.WhereFilter>   _where;
	readonly Func<Sort, Syncfusion.Blazor.Data.Sort>                 _sort;

	public DataManagerRequests(
		Func<SearchFilter, Syncfusion.Blazor.Data.SearchFilter> search,
		Func<WhereFilter, Syncfusion.Blazor.Data.WhereFilter> where,
		Func<Sort, Syncfusion.Blazor.Data.Sort> sort)
	{
		_search = search;
		_where  = where;
		_sort   = sort;
	}

	public DataManagerRequest Get(PageRequest parameter)
	{
		var (search, where, sorting, filters, includeTotalCount, partition) = parameter;

		return new()
		{
			Skip           = partition?.Skip ?? 0,
			Take           = partition?.Top ?? DefaultPageSize.Default,
			RequiresCounts = includeTotalCount,
			Search         = search.Select(_search).ToList(),
			Where          = where.Select(_where).ToList(),
			Select         = filters.ToList(),
			Sorted         = sorting.Select(_sort).ToList()
		};
	}
}