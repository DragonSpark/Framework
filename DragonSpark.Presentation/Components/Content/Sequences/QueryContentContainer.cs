using DragonSpark.Application.Entities.Queries.Runtime;
using DragonSpark.Application.Entities.Queries.Runtime.Pagination;
using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Components;
using System;

namespace DragonSpark.Presentation.Components.Content.Sequences;

partial class QueryContentContainer<T> : IReportedTypeAware
{
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
	public ICondition<IPages<T>> Results { get; set; } = HasResults<T>.Default;

	[Parameter]
	public Type? ReportedType { get; set; }

	[Parameter]
	public RenderFragment<IPages<T>>? HeaderTemplate { get; set; }

	[Parameter]
	public RenderFragment<IPages<T>>? FooterTemplate { get; set; }

	IResulting<IPages<T>>? Subject { get; set; }

	protected override void OnParametersSet()
	{
		base.OnParametersSet();
		Subject ??= DetermineSubject();
	}

	IResulting<IPages<T>> DetermineSubject()
		=> Content != null
			   ? Pagination.Then().Bind(new PagingInput<T>(this, Content, Compose)).Out()
			   : EmptyPaging<T>.Default;

	public Type Get() => ReportedType ?? GetType();
}