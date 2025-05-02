using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Components;
using System;

namespace DragonSpark.Presentation.Components.Content.Sequences;

public abstract class PagingContainer<T> : Templates.ManyActiveContentTemplateComponentBase<IPages<T>>
{
	[Parameter]
	public ICondition<bool?> Results { get; set; } = HasResults.Default;

	[Parameter]
	public Type? ReportedType { get; set; }

	[Parameter]
	public RenderFragment<PagingRenderState<T>>? HeaderTemplate { get; set; }

	[Parameter]
	public RenderFragment<PagingRenderState<T>>? FooterTemplate { get; set; }
}