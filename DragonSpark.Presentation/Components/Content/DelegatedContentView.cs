using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	public class DelegatedContentView<TValue> : ComponentBase
	{
		protected override async Task OnParametersSetAsync()
		{
			Fragment ??= await DetermineFragment();
		}


		async ValueTask<RenderFragment> DetermineFragment()
		{
			Fragment = LoadingTemplate; // Re-entry is occurring for some reason.
			try
			{
				var operation = LoadContent();
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

		protected virtual ValueTask<TValue> LoadContent() => Content.Get();

		RenderFragment? Fragment { get; set; }

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (Fragment == null)
			{
				builder.AddContent(0, LoadingTemplate);
			}
			else
			{
				builder.AddContent(1, Fragment);
			}

		}

		[Parameter]
		public ActiveContent<TValue> Content
		{
			get => _content;
			set
			{
				if (_content != value)
				{
					_content = value;
					Fragment = default;
				}
			}
		}	ActiveContent<TValue> _content = default!;

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