﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components
{
	public sealed class ActiveView<TValue> : ComponentBase
	{
		protected override async Task OnInitializedAsync()
		{
			var operation = Source.Get();
			if (!operation.IsCompleted)
			{
				await operation;
			}
			await base.OnInitializedAsync().ConfigureAwait(false);
		}

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (Source?.HasValue ?? false)
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

		[Parameter]
		public ActiveResult<TValue> Source { get; set; }

		[Parameter]
		public RenderFragment<TValue> ChildContent { get; set; }

		[Parameter]
		public RenderFragment LoadingTemplate { get; set; } = x => x.AddContent(2, "Loading, please wait.");

		[Parameter]
		public RenderFragment NotAssignedTemplate { get; set; } = x => x.AddContent(2, "This view's required information does not exist.");
	}
}