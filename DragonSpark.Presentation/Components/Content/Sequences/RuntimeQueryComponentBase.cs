using DragonSpark.Application.AspNet.Entities.Queries.Runtime;
using DragonSpark.Compose;
using DragonSpark.Presentation.Components.Eventing;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Sequences;

public abstract class RuntimeQueryComponentBase<T> : InstanceComponentBase<IQueries<T>>
{
	[Inject]
	IPublisher<RefreshObjectMessage> Publisher { get; set; } = null!;

	protected override void RequestNewContent(bool redraw = false) {}

	protected override async ValueTask RefreshState()
	{
		var instance = Instance;
		if (instance is not null)
		{
			await Publisher.Get(new(instance));
		}
		await base.RefreshState().Off();
	}
}