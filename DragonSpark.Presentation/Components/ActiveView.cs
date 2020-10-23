﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
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
				try
				{
					await operation;
				}
				// ReSharper disable once CatchAllClause
				catch (Exception e)
				{
					EncounteredException = true;
					await Exceptions.Get((GetType(), e));
				}
			}
		}

		bool EncounteredException { get; set; }

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (EncounteredException)
			{
				builder.AddContent(0, ExceptionTemplate ?? NotAssignedTemplate);
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
		public RenderFragment LoadingTemplate { get; set; } = x => x.AddContent(3, "Loading, please wait.");

		[Parameter]
		public RenderFragment NotAssignedTemplate { get; set; } = x => x.AddContent(1, "This view's required information does not exist.");

		[Parameter]
		public RenderFragment? ExceptionTemplate { get; set; }
	}
}