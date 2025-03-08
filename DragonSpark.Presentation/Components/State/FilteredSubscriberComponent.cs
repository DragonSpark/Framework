using DragonSpark.Application.Connections.Events;
using DragonSpark.Model;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

public class FilteredSubscriberComponent<T> : FilteredSubscriptionComponent<T> where T : notnull
{
	[Parameter]
	public uint? Recipient { get; set; }

	[Parameter]
	public ISubscriber<T> Registration { get; set; } = null!;

	protected override ISubscription DetermineSubscription() => Registration.Get(new(Recipient, OnReceive));
}

public sealed class FilteredSubscriberComponent : FilteredSubscriptionComponent<None>
{
	[Parameter]
	public uint? Recipient { get; set; }

	[Parameter]
	public ISubscriber Registration { get; set; } = null!;

	protected override ISubscription DetermineSubscription() => Registration.Get(new(Recipient, OnReceive));

	Task OnReceive() => OnReceive(None.Default);
}