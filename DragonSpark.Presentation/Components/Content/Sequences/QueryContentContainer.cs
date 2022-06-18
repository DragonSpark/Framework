using DragonSpark.Application;
using DragonSpark.Application.Entities.Queries.Runtime;
using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components;
using System;

namespace DragonSpark.Presentation.Components.Content.Sequences;

partial class QueryContentContainer<T>
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
	public Type? ReportedType { get; set; }

	[Parameter]
	public RenderFragment<IPages<T>>? HeaderTemplate { get; set; }

	[Parameter]
	public RenderFragment<IPages<T>>? FooterTemplate { get; set; }

	IResulting<IPages<T>?>? Subject { get; set; }

	IPaging<T> Paging { get; set; } = Paging<T>.Default; // TODO

	protected override void OnParametersSet()
	{
		base.OnParametersSet();
		Subject ??= DetermineSubject();
	}

	IResulting<IPages<T>?> DetermineSubject()
	{
		if (Content != null)
		{
			var selector = Paging.Then().AccountOut().Bind(new PagingInput<T>(Content, Compose)).Operation();
			var subject  = ReportedType != null ? selector.Then().Handle(Exceptions, ReportedType) : selector;
			var result   = subject.Out();
			return result;
		}

		return Defaulting<IPages<T>>.Default;
	}
}