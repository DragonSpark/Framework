using Microsoft.Extensions.Hosting;
using NetFabric.Hyperlinq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

public sealed class InitializeStorageService : IHostedService
{
	readonly IEnumerable<IContainer> _containers;

	public InitializeStorageService(IEnumerable<IContainer> containers) => _containers = containers;

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		foreach (var container in _containers.AsValueEnumerable().Select(x => x.Get()))
		{
			await container.CreateIfNotExistsAsync().ConfigureAwait(false);
		}
	}

	public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}