using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Components.Server;

namespace DragonSpark.Application.Hosting.Server.Blazor;

sealed class OptimizedCircuitConfiguration : ICommand<CircuitOptions>
{
	public static OptimizedCircuitConfiguration Default { get; } = new();

	OptimizedCircuitConfiguration() {}

	public void Execute(CircuitOptions parameter)
	{
		parameter.DisconnectedCircuitRetentionPeriod         = TimeSpan.FromSeconds(90);
		parameter.PersistedCircuitInMemoryRetentionPeriod    = TimeSpan.FromMinutes(30);
		parameter.PersistedCircuitDistributedRetentionPeriod = TimeSpan.FromHours(2);
	}
}