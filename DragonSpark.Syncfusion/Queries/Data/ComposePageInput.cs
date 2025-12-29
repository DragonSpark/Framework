using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Selection;
using Syncfusion.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;

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

sealed class ComposeSort : ISelect<Syncfusion.Blazor.Data.Sort, Sort>
{
	public static ComposeSort Default { get; } = new();

	ComposeSort() {}

	public Sort Get(Syncfusion.Blazor.Data.Sort parameter) => new(parameter.Name, parameter.Direction);
}

public sealed record Empty(
	IReadOnlyCollection<SearchFilter> Search,
	IReadOnlyCollection<WhereFilter> Where,
	IReadOnlyCollection<Sort> Sort,
	IReadOnlyCollection<string> Filter)
{
	public static Empty Default { get; } = new();

	Empty() : this([], [], [], []) {}
}

sealed class ComposeSearchFilter : ISelect<Syncfusion.Blazor.Data.SearchFilter, SearchFilter>
{
	public static ComposeSearchFilter Default { get; } = new();

	ComposeSearchFilter() {}

	public SearchFilter Get(Syncfusion.Blazor.Data.SearchFilter parameter)
		=> new(parameter.Fields, parameter.Key.Account() ?? string.Empty,
		       parameter.Operator.Account() ?? "contains", parameter.IgnoreCase, parameter.IgnoreAccent);
}

sealed class ComposeWhereFilter : ISelect<Syncfusion.Blazor.Data.WhereFilter, WhereFilter>
{
	public static ComposeWhereFilter Default { get; } = new();

	ComposeWhereFilter() {}

	public WhereFilter Get(Syncfusion.Blazor.Data.WhereFilter parameter)
		=> new(parameter.Field.Account() ?? string.Empty,
		       parameter.IgnoreCase,
		       parameter.IgnoreAccent,
		       parameter.IsComplex,
		       parameter.Operator.Account() ?? "equal",
		       parameter.Condition.Account() ?? "and",
		       parameter.value,
		       predicates: parameter.predicates.Account()?.Select(Get).ToList() ?? []);
}