using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Polly;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	class Class1 {}

	public sealed class ApplicationContentView<TValue> : PolicyAwareDelegatedContentView<TValue>
	{
		public ApplicationContentView() : base(DurableApplicationContentPolicy.Default.Get()) {}
	}

	public class PolicyAwareDelegatedContentView<TValue> : DelegatedContentView<TValue>
	{
		public PolicyAwareDelegatedContentView() : this(Polly.Policy.Handle<Exception>().RetryAsync()) {}

		public PolicyAwareDelegatedContentView(IAsyncPolicy policy) => Policy = policy;

		protected override void OnInitialized()
		{
			base.OnInitialized();
			Body = Start.A.Result(base.LoadContent).Select(x => x.AsTask());
		}

		[Parameter]
		public IAsyncPolicy Policy { get; set; }

		Func<Task<TValue>> Body { get; set; } = default!;

		protected override ValueTask<TValue> LoadContent() => Policy.ExecuteAsync(Body).ToOperation();
	}

	public class DelegatedContentView<TValue> : ComponentBase
	{
		protected override async Task OnParametersSetAsync()
		{
			Fragment = default;
			Fragment = await DetermineFragment();
		}

		async ValueTask<RenderFragment> DetermineFragment()
		{
			try
			{
				var operation = LoadContent();
				var content   = operation.IsCompletedSuccessfully ? operation.Result : await operation;
				var result    = content is not null ? ChildContent(content) : NotAssignedTemplate;
				return result;
			}
			// ReSharper disable once CatchAllClause
			catch
			{
				return ExceptionTemplate;
			}
		}

		protected virtual ValueTask<TValue> LoadContent() => Content.Get();

		RenderFragment? Fragment { get; set; }

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (Fragment != null)
			{
				builder.AddContent(0, Fragment ?? LoadingTemplate);
			}
			else
			{
				builder.AddContent(1, LoadingTemplate);
			}
		}

		[Parameter]
		public ActiveContent<TValue> Content { get; set; } = default!;

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