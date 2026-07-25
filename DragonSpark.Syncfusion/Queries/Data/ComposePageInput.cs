using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Selection;
using Syncfusion.Blazor;

namespace DragonSpark.SyncfusionRendering.Queries.Data;

public sealed class ComposePageInput : ISelect<DataManagerRequest, PageRequest>
{
	public static ComposePageInput Default { get; } = new();

	ComposePageInput()
		: this(ComposeSearchFilter.Default.Get, ComposeWhereFilter.Default.Get, ComposeSort.Default.Get,
		       Empty.Default) {}

	readonly Func<Syncfusion.Blazor.Data.SearchFilter, SearchFilter> _search;
	readonly Func<Syncfusion.Blazor.Data.WhereFilter, WhereFilter>   _where;
	readonly Func<Syncfusion.Blazor.Data.Sort, Sort>                 _sort;
	readonly Empty                                                   _empty;

	// ReSharper disable once TooManyDependencies
	public ComposePageInput(Func<Syncfusion.Blazor.Data.SearchFilter, SearchFilter> search,
	                        Func<Syncfusion.Blazor.Data.WhereFilter, WhereFilter> where,
	                        Func<Syncfusion.Blazor.Data.Sort, Sort> sort,
	                        Empty empty)
	{
		_search = search;
		_where  = where;
		_sort   = sort;
		_empty  = empty;
	}

	public PageRequest Get(DataManagerRequest parameter)
		=> new(parameter.Search.Account()?.Select(_search).ToList().AsReadOnly() ?? _empty.Search,
		       parameter.Where.Account()?.Select(_where).ToList().AsReadOnly() ?? _empty.Where,
		       parameter.Sorted.Account()?.Select(_sort).ToList().AsReadOnly() ?? _empty.Sort,
		       parameter.Select.Account()?.AsReadOnly() ?? _empty.Filter,
		       parameter.RequiresCounts,
		       parameter.Partition());
}