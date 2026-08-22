using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Hosting.Server.Blazor;

sealed class DistributedAwareServiceConfiguration : ICommand<IServiceCollection>
{
	readonly ICommand<IServiceCollection> _previous;

	public DistributedAwareServiceConfiguration(byte receive)
		: this(OptimizedCircuitConfiguration.Default.Execute, receive) {}

	public DistributedAwareServiceConfiguration(Action<CircuitOptions> options, byte receive)
		: this(new DefaultServiceConfiguration(options, receive)) {}

	public DistributedAwareServiceConfiguration(ICommand<IServiceCollection> previous) => _previous = previous;

	public void Execute(IServiceCollection parameter)
	{
		parameter.AddHybridCache();
		parameter.AddOptions<CircuitOptions>()
		         .Configure<HybridCache>((options, subject) => options.HybridPersistenceCache = subject);
		_previous.Execute(parameter);
	}
}