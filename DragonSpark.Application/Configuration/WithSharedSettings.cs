using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Application.Configuration;

sealed class WithSharedSettings : IAlteration<IHostBuilder>
{
	public static WithSharedSettings Default { get; } = new();

	WithSharedSettings() : this(ApplySharedSettings.Default.Execute) {}

	readonly Action<HostBuilderContext, IConfigurationBuilder> _configure;

	public WithSharedSettings(Action<HostBuilderContext, IConfigurationBuilder> configure) => _configure = configure;

	public IHostBuilder Get(IHostBuilder parameter) => parameter.ConfigureAppConfiguration(_configure);
}