using DragonSpark.Application.Entities.Queries.Runtime;
using DragonSpark.Application.Entities.Queries.Runtime.Pagination;
using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Components;
using System;

namespace DragonSpark.Presentation.Components.Content.Sequences;

partial class QueryContentContainer<T> : IReportedTypeAware, IPageContainer<T>
{
	bool?                       _results;
	readonly IMutable<Page<T>?> _current = new CurrentPage<T>();

	[Parameter]
	public IQueries<T>? Content
	{
		get => _content;
		set
		{
			if (_content != value)
			{
				_content = value;
				Subject  = null;
				_results = null;
			}
		}
	}	IQueries<T>? _content;

	[Parameter]
	public ICompose<T> Compose
	{
		get => _compose;
		set
		{
			if (_compose != value)
			{
				_compose = value;
				Subject  = null;
			}
		}
	}	ICompose<T> _compose = DefaultCompose<T>.Default;

	[Parameter]
	public ICondition<bool?> Results { get; set; } = HasResults.Default;

	[Parameter]
	public Type? ReportedType { get; set; }

	[Parameter]
	public RenderFragment<IPages<T>>? HeaderTemplate { get; set; }

	[Parameter]
	public RenderFragment<IPages<T>>? FooterTemplate { get; set; }

	[Parameter]
	public IPagination<T>? Pagination { get; set; }

	IPages<T>? Subject { get; set; }

	protected override void OnParametersSet()
	{
		base.OnParametersSet();
		Subject ??= Content is not null ? DetermineSubject(Content) : EmptyPages<T>.Default;
	}

	IPages<T> DetermineSubject(IQueries<T> parameter)
	{
		var start = Paging.Get(new(this, parameter, Compose));
		return Pagination?.Get(start) ?? start;
	}

	public Type Get() => ReportedType ?? GetType();

	void ICommand<Page<T>>.Execute(Page<T> parameter)
	{
		_results ??= parameter.Total is > 0;
		_current.Execute(parameter);
		StateHasChanged();
	}
}