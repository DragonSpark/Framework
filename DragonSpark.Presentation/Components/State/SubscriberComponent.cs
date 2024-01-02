using DragonSpark.Application.Connections.Client;
using DragonSpark.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

public sealed class SubscriberComponent<T> : ReceiveParameterViewComponent<T> where T : notnull
{
	Func<T, Task> _body = default!;

	protected override void OnInitialized()
	{
		_body = OnReceive;
		base.OnInitialized();
	}

	[Parameter]
	public uint? Recipient { get; set; }

	[Parameter]
	public ISubscriber<T> Registration { get; set; } = default!;

	protected override ISubscription DetermineSubscription() => Registration.Get(new(Recipient, _body));
}

public sealed class SubscriberOfComponent : ReceiveParameterViewComponent
{
	Func<Task> _body = default!;

	protected override void OnInitialized()
	{
		_body = OnReceive;
		base.OnInitialized();
	}

	[Parameter]
	public uint? Recipient { get; set; }

	[Parameter]
	public ISubscriber Registration { get; set; } = default!;

	protected override ISubscription DetermineSubscription() => Registration.Get(new(Recipient, _body));

	Task OnReceive() => OnReceive(None.Default);
}