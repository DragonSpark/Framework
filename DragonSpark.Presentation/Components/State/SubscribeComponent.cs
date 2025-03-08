using DragonSpark.Application.Connections.Events;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.State;

public sealed class SubscribeComponent<T> : SubscriptionComponent<T> where T : notnull
{
	[Parameter]
	public ISubscribe<T> Registration { get; set; } = null!;

	protected override ISubscription DetermineSubscription() => Registration.Get(OnReceive);
}