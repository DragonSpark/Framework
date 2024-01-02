﻿using DragonSpark.Application.Connections.Client;
using DragonSpark.Model;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

public class FilteredSubscriberComponent<T> : FilteredReceiveParameterViewComponent<T> where T : notnull
{
	[Parameter]
	public uint? Recipient { get; set; }

	[Parameter]
	public ISubscriber<T> Registration { get; set; } = default!;

	protected override ISubscription DetermineSubscription() => Registration.Get(new(Recipient, OnReceive));
}

public sealed class FilteredSubscriberComponent : FilteredReceiveParameterViewComponent<None>
{
	[Parameter]
	public uint? Recipient { get; set; }

	[Parameter]
	public ISubscriber Registration { get; set; } = default!;

	protected override ISubscription DetermineSubscription() => Registration.Get(new(Recipient, OnReceive));

	Task OnReceive() => OnReceive(None.Default);
}