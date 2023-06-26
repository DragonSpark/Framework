using DragonSpark.Application.Entities.Queries.Runtime;
using DragonSpark.Application.Entities.Queries.Runtime.Pagination;
using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Components;
using System;

namespace DragonSpark.Presentation.Components.Content.Sequences;

partial class QueryContentContainer<T> : IReportedTypeAware, IAnyComposer<T>
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
	public IAnyComposer<T>? Any { get; set; }

	[Parameter]
	public Type? ReportedType { get; set; }

	[Parameter]
	public RenderFragment<IPages<T>>? HeaderTemplate { get; set; }

	[Parameter]
	public RenderFragment<IPages<T>>? FooterTemplate { get; set; }

	[Parameter]
	public IPaginationComposer<T>? Pagination { get; set; }

	IResulting<IPages<T>>? Subject { get; set; }

	protected override void OnParametersSet()
	{
		base.OnParametersSet();
		Subject ??= DetermineSubject();
	}

	IResulting<IPages<T>> DetermineSubject()
	{
		if (Content != null)
		{
			var start = DefaultPagination.Then().Bind(new PagingInput<T>(this, Content, Compose)).Out();
			return Pagination?.Get(start) ?? start;
		}
		return EmptyPaging<T>.Default;
	}


	public Type Get() => ReportedType ?? GetType();

	IAny<T>? ISelect<IAny<T>, IAny<T>?>.Get(IAny<T> parameter) => Any?.Get(parameter);
}