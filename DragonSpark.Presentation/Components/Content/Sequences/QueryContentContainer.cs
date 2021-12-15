using DragonSpark.Application;
using DragonSpark.Application.Entities.Queries.Runtime;
using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.Content.Rendering;
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

	IResulting<IPaging<T>?>? Subject { get; set; }

	protected override void OnInitialized()
	{
		Paging = new Pagings<T>(new PreRenderAwarePagers<T>(Builder, new QueryInputKey(Identification.Get(this))));
		base.OnInitialized();
	}

	Pagings<T> Paging { get; set; } = default!;

	[Inject]
	IRenderContentKey Identification { get; set; } = default!;

	[Inject]
	PreRenderingAwarePagerBuilder<T> Builder { get; set; } = default!;

	protected override void OnParametersSet()
	{
		base.OnParametersSet();
		Subject ??= DetermineSubject();
	}

	IResulting<IPaging<T>?> DetermineSubject()
	{
		if (Content != null)
		{
			var selector = Paging.Then().AccountOut().Bind(new PagingInput<T>(Content, Compose));
			var subject  = ReportedType != null ? selector.Then().Handle(Exceptions, ReportedType) : selector;
			var result   = subject.Out();
			return result;
		}

		return Defaulting<IPaging<T>>.Default;
	}
}