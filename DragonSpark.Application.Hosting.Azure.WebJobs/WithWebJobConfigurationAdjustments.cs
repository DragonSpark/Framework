using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

sealed class WithWebJobConfigurationAdjustments : ICommand<IHostBuilder>
{
	public static WithWebJobConfigurationAdjustments Default { get; } = new();

	WithWebJobConfigurationAdjustments() : this(AdjustWebJobConfigurations.Default.Execute) {}

	readonly Action<HostBuilderContext, IConfigurationBuilder> _adjust;

	public WithWebJobConfigurationAdjustments(Action<HostBuilderContext, IConfigurationBuilder> adjust)
		=> _adjust = adjust;

	public void Execute(IHostBuilder parameter)
	{
		parameter.ConfigureAppConfiguration(_adjust);
	}
}