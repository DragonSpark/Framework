using DragonSpark.Model.Commands;
using DragonSpark.Presentation.Environment;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Presentation;

sealed class AddSignalR : ICommand<IServiceCollection>
{
	public static AddSignalR Default { get; } = new();

	AddSignalR() : this(x => x.AddFilter<ComponentContextFilter>()) {}

	readonly Action<HubOptions> _configure;

	public AddSignalR(Action<HubOptions> configure) => _configure = configure;

	public void Execute(IServiceCollection parameter)
	{
		parameter.AddSignalR(_configure);
	}
}