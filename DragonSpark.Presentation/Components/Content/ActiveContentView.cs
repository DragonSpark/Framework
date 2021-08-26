﻿using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	public class ActiveContentView<TValue> : ComponentBase
	{
		protected override async Task OnParametersSetAsync()
		{
			if (Render == null)
			{
				Render = Saved.Account() ?? LoadingTemplate;
				Saved  = Render = await DetermineFragment();
			}
		}

		async ValueTask<RenderFragment> DetermineFragment()
		{
			try
			{
				var operation = Content.Get();
				var content   = operation.IsCompletedSuccessfully ? operation.Result : await operation;
				var result    = content is not null ? ChildContent(content) : NotAssignedTemplate;
				return result;
			}
			catch (Exception error)
			{
				await Exceptions.Get(GetType(), error);

				return ExceptionTemplate;
			}
		}

		RenderFragment? Render { get; set; }

		RenderFragment Saved { get; set; } = default!;

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.AddContent(0, Render);
		}

		[Parameter]
		public IActiveContent<TValue> Content
		{
			get => _content;
			set
			{
				if (_content != value)
				{
					_content = value;
					Render   = default;
				}
			}
		}	IActiveContent<TValue> _content = default!;

		[Parameter]
		public RenderFragment<TValue> ChildContent { get; set; } = default!;

		[Parameter]
		public RenderFragment LoadingTemplate
		{
			get => _loadingTemplate;
			set
			{
				if (_loadingTemplate != value)
				{
					Saved = _loadingTemplate = value;
				}
			}
		}	RenderFragment _loadingTemplate = DefaultLoadingTemplate.Default;

		[Parameter]
		public RenderFragment NotAssignedTemplate { get; set; } = DefaultNotAssignedTemplate.Default;

		[Parameter]
		public RenderFragment ExceptionTemplate { get; set; } = DefaultExceptionTemplate.Default;
	}
}