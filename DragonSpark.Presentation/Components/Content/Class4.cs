using DragonSpark.Application.Entities.Queries.Runtime;
using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Presentation.Components.Eventing;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	internal class Class4
	{
	}

	public sealed record RefreshPagingMessage<T>(IPaging<T> Subject);

	public class PagingMonitor<T> : ComponentBase, IHandle<RefreshQueriesMessage<T>>, IDisposable
	{
		protected override void OnInitialized()
		{
			base.OnInitialized();
			Events.Subscribe(this);
		}

		[Parameter]
		public IQueries<T> Topic { get; set; } = default!;

		[Parameter]
		public IPaging<T> Subject { get; set; } = default!;

		[Inject]
		IEventAggregator Events { get; set; } = default!;

		[Inject]
		IPublisher<RefreshPagingMessage<T>> Publisher { get; set; } = default!;

		public Task HandleAsync(RefreshQueriesMessage<T> message)
			=> message.Subject == Topic ? Publisher.Get(new(Subject)).AsTask() : Task.CompletedTask;

		public void Dispose()
		{
			Events.Unsubscribe(this);
		}
	}

}
