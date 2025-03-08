using DragonSpark.Application.Connections.Events;
using DragonSpark.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

public sealed class SubscriberComponent<T> : SubscriptionComponent<T> where T : notnull
{
	Func<T, Task> _body = null!;

	protected override void OnInitialized()
	{
		_body = OnReceive;
		base.OnInitialized();
	}

	[Parameter]
	public uint? Recipient { get; set; }

	[Parameter]
	public ISubscriber<T> Registration { get; set; } = null!;

	protected override ISubscription DetermineSubscription() => Registration.Get(new(Recipient, _body));
}

public sealed class SubscriberOfComponent : SubscriptionComponent
{
	Func<Task> _body = null!;

	protected override void OnInitialized()
	{
		_body = OnReceive;
		base.OnInitialized();
	}

	[Parameter]
	public uint? Recipient { get; set; }

	[Parameter]
	public ISubscriber Registration { get; set; } = null!;

	protected override ISubscription DetermineSubscription() => Registration.Get(new(Recipient, _body));

	Task OnReceive() => OnReceive(None.Default);
}