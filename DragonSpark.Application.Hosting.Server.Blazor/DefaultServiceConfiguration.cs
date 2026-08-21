using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Hosting.Server.Blazor;

sealed class DefaultServiceConfiguration : ICommand<IServiceCollection>
{
	public static DefaultServiceConfiguration Default { get; } = new();

	DefaultServiceConfiguration() : this(32) {}

	readonly Action<CircuitOptions> _options;
	readonly byte                   _receive;

	public DefaultServiceConfiguration(byte receive) : this(_ => {}, receive) {}

	public DefaultServiceConfiguration(Action<CircuitOptions> options, byte receive)
	{
		_options = options;
		_receive = receive;
	}

	public void Execute(IServiceCollection parameter)
	{
		parameter.AddRazorComponents()
		         .AddInteractiveServerComponents(_options)
		         .AddHubOptions(x => x.MaximumReceiveMessageSize = _receive * 1024);
	}
}

// TODO
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