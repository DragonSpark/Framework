﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components
{
	public class ActiveView<TValue> : ComponentBase
	{
		protected override async Task OnParametersSetAsync()
		{
			var operation = Source.Get();

			try
			{
				if (!operation.IsCompleted)
				{
					await operation;
				}
			}
			// ReSharper disable once CatchAllClause
			catch
			{
				Fragment = ExceptionTemplate;
			}
		}

		RenderFragment? Fragment { get; set; }

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (Fragment != null)
			{
				builder.AddContent(0, Fragment);
			}
			else
			{
				if (Source.HasValue)
				{
					var value = Source.Value;
					if (value is null)
					{
						builder.AddContent(1, NotAssignedTemplate);
					}
					else
					{
						builder.AddContent(2, ChildContent(value));
					}
				}
				else
				{
					builder.AddContent(3, LoadingTemplate);
				}
			}
		}


		[Parameter]
		public ActiveResult<TValue> Source { get; set; } = default!;

		[Parameter]
		public RenderFragment<TValue> ChildContent { get; set; } = default!;

		[Parameter]
		public RenderFragment LoadingTemplate { get; set; } = DefaultLoadingTemplate.Default;

		[Parameter]
		public RenderFragment NotAssignedTemplate { get; set; } = DefaultNotAssignedTemplate.Default;

		[Parameter]
		public RenderFragment ExceptionTemplate { get; set; } = DefaultExceptionTemplate.Default;
	}
}