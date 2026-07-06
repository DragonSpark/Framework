using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using Microsoft.AspNetCore.Components;
using System;

namespace DragonSpark.Presentation.Components.Content.Sequences;

public abstract class PagingContainer<T> : Templates.ManyActiveContentTemplateComponentBase<IPages<T>>
{
	[Parameter]
	public Type? ReportedType { get; set; }

	[Parameter]
	public RenderFragment<PagingRenderState>? HeaderTemplate { get; set; }

	[Parameter]
	public RenderFragment<PagingRenderState>? FooterTemplate { get; set; }
}