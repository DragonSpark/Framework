using DragonSpark.Model.Commands;
using LightInject;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Composition;

class ConfigureContainer : ICommand<IHostBuilder>
{
	readonly Action<IServiceContainer> _configure;

	public ConfigureContainer(Action<IServiceContainer> configure) => _configure = configure;

	public void Execute(IHostBuilder parameter)
	{
		parameter.ConfigureContainer(_configure);
	}
}

sealed class ConfigureContainer<T> : ConfigureContainer where T : ICompositionRoot, new()
{
	public static ConfigureContainer<T> Default { get; } = new();

	ConfigureContainer() : base(x => x.RegisterFrom<T>()) {}
}