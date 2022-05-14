using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

sealed class Hosting : ICommand<IHostBuilder>
{
	readonly Action<QueuesOptions> _queues;

	public Hosting(Action<QueuesOptions> queues) => _queues = queues;

	public void Execute(IHostBuilder parameter)
	{
		parameter.ConfigureWebJobs(x => x.AddAzureStorageCoreServices()
		                                 .AddAzureStorageBlobs()
		                                 .AddAzureStorageQueues(_queues));
	}
}

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

sealed class AdjustWebJobConfigurations : ICommand<(HostBuilderContext, IConfigurationBuilder)>
{
	public static AdjustWebJobConfigurations Default { get; } = new();

	AdjustWebJobConfigurations() : this(ArrayPool<IConfigurationSource>.Shared) {}

	readonly ArrayPool<IConfigurationSource> _pool;

	public AdjustWebJobConfigurations(ArrayPool<IConfigurationSource> pool) => _pool = pool;

	public void Execute((HostBuilderContext, IConfigurationBuilder) parameter)
	{
		var (_, builder) = parameter;
		using var items = builder.Sources.ToArray()[..^2].AsValueEnumerable().ToArray(_pool);
		builder.Sources.Clear();
		builder.Sources.To<List<IConfigurationSource>>().AddRange(items);
	}
}