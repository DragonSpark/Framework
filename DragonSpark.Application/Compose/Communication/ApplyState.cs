using DragonSpark.Application.Communication;
using DragonSpark.Application.Security;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace DragonSpark.Application.Compose.Communication;

sealed class ApplyState : ICommand<(IServiceProvider, HttpClient)>
{
	public static ApplyState Default { get; } = new ApplyState();

	ApplyState() : this(CookieHeader.Default) {}

	readonly string _name;

	public ApplyState(string name) => _name = name;

	public void Execute((IServiceProvider, HttpClient) parameter)
	{
		var (provider, client) = parameter;

		var values = new ClientStateValues(provider.GetRequiredService<ICurrentContext>()).Get().Open();

		client.DefaultRequestHeaders.Add(_name, values);
	}
}