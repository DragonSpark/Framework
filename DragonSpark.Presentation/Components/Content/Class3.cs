using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	class Class3 {}

	public class ManyActiveContentTemplateComponentBase<T> : ActiveContentTemplateComponentBase<T>
	{
		[Parameter]
		public RenderFragment? NoElementsFoundTemplate { get; set; }
	}

	public class ActiveContentTemplateComponentBase<T> : ContentTemplateComponentBase<T>
	{
		[Parameter]
		public RenderFragment LoadingTemplate { get; set; } = DefaultLoadingTemplate.Default;
	}

	public class ContentTemplateComponentBase<T> : ComponentBase
	{
		[Parameter]
		public RenderFragment<T> ChildContent { get; set; } = default!;

		[Parameter]
		public RenderFragment NotAssignedTemplate { get; set; } = DefaultNotAssignedTemplate.Default;

		[Parameter]
		public RenderFragment ExceptionTemplate { get; set; } = DefaultExceptionTemplate.Default;
	}

	public class ReportedRadzenPaging<T> : ReportedAllocated<LoadDataArgs>, IRadzenPaging<T>
	{
		readonly IRadzenPaging<T> _previous;

		public ReportedRadzenPaging(IRadzenPaging<T> previous, Action<Task> report) : base(previous, report)
			=> _previous = previous;

		public ulong Count => _previous.Count;

		public IEnumerable<T> Current => _previous.Current;
	}

}