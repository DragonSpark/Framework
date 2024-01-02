using DragonSpark.Application.Connections.Client;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.State;

public sealed class SubscribeComponent<T> : SubscriptionComponent<T> where T : notnull
{
	[Parameter]
	public ISubscribe<T> Registration { get; set; } = default!;

	protected override ISubscription DetermineSubscription() => Registration.Get(OnReceive);
}