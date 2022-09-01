using DragonSpark.Application.Entities.Queries.Runtime;
using DragonSpark.Presentation.Components.Eventing;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Sequences;

public abstract class RuntimeQueryComponentBase<T> : InstanceComponentBase<IQueries<T>>
{
	[Inject]
	IPublisher<RefreshQueriesMessage<T>> Publisher { get; set; } = default!;

	protected override void RequestNewContent() {}

	protected override async ValueTask RefreshState()
	{
		var instance = Instance;
		if (instance is not null)
		{
			await Publisher.Get(new(instance));
		}
		await base.RefreshState().ConfigureAwait(false);
	}
}