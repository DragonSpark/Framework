using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Application.Configuration;

sealed class WithAmbientConfiguration : IAlteration<IHostBuilder>
{
	public static WithAmbientConfiguration Default { get; } = new();

	WithAmbientConfiguration() : this(ApplyAmbientConfiguration.Default.Execute) {}

	readonly Action<HostBuilderContext, IConfigurationBuilder> _configure;

	public WithAmbientConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configure)
		=> _configure = configure;

	public IHostBuilder Get(IHostBuilder parameter) => parameter.ConfigureAppConfiguration(_configure);
}