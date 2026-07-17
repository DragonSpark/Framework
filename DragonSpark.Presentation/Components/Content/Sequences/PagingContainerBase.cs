using DragonSpark.Application.AspNet.Entities.Queries.Runtime;
using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;
using DragonSpark.Presentation.Components.Content.Templates;
using Microsoft.AspNetCore.Components;
using System;

namespace DragonSpark.Presentation.Components.Content.Sequences;

public abstract class PagingContainerBase<T> : ActiveContentTemplateComponentBase<IPages<T>>
{
	[Parameter]
	public Type? ReportedType { get; set; }

	[Parameter]
	public IQueries<T>? Content { get; set; }

	[Parameter]
	public ICompose<T> Compose { get; set; } = DefaultCompose<T>.Default;

	[Parameter]
	public IPagination<T>? Pagination { get; set; }

	[Parameter]
	public RenderFragment<PagingRenderState>? HeaderTemplate { get; set; }

	[Parameter]
	public RenderFragment<PagingRenderState>? FooterTemplate { get; set; }

	[Parameter]
	public virtual RenderFragment? EmptyElementsTemplate { get; set; } = DefaultEmptySequenceTemplate.Default;
}