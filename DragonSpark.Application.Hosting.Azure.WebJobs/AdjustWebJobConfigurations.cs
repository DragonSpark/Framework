using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NetFabric.Hyperlinq;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

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