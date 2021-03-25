using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using Polly;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
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
}