using Aspire.Hosting;
using DragonSpark.Application.Configuration;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Application.Hosting.Aspire;

public class Class1;

public static class Extensions
{
	public static IDistributedApplicationBuilder WithAmbientConfiguration(this IDistributedApplicationBuilder @this)
		=> @this.To(Aspire.WithAmbientConfiguration.Default);
}

sealed class WithAmbientConfiguration : IAlteration<IDistributedApplicationBuilder>
{
	public static WithAmbientConfiguration Default { get; } = new();

	WithAmbientConfiguration() : this(ApplyAmbientConfiguration.Default.Execute) {}

	readonly Action<IHostEnvironment, IConfigurationBuilder> _configure;

	public WithAmbientConfiguration(Action<IHostEnvironment, IConfigurationBuilder> configure)
		=> _configure = configure;

	public IDistributedApplicationBuilder Get(IDistributedApplicationBuilder parameter)
	{
		_configure(parameter.Environment, parameter.Configuration);
		return parameter;
	}
}