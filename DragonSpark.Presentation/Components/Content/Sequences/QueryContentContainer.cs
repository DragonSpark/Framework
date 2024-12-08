using DragonSpark.Application.AspNet.Entities.Queries.Runtime;
using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Components;
using System;

namespace DragonSpark.Presentation.Components.Content.Sequences;

partial class QueryContentContainer<T> : IPageContainer<T>
{
	Switch     _error = new();
	bool?      _results;
	IPages<T>? _subject;

	[Parameter]
	public IQueries<T>? Content
	{
		get => _content;
		set
		{
			if (_content != value)
			{
				_content = value;
				_subject = null;
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
				_subject = null;
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

	protected override void OnParametersSet()
	{
		base.OnParametersSet();
		_subject ??= DetermineSubject(Content.Verify());
	}

	IPages<T> DetermineSubject(IQueries<T> parameter)
	{
		var start = Paging.Get(new(this, parameter, Compose));
		return Pagination?.Get(start) ?? start;
	}

	public Type Get() => ReportedType ?? GetType();

	void ICommand<Page<T>>.Execute(Page<T> parameter)
	{
		_error.Down();
		_results ??= parameter.Total is > 0;
		StateHasChanged();
	}

	void ICommand<Exception>.Execute(Exception parameter)
	{
		_error.Up();
		_results = null;
		StateHasChanged();
	}
}