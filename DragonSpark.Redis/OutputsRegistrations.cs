using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Redis;

sealed class OutputsRegistrations : ICommand<IServiceCollection>
{
	readonly string? _name;

	public OutputsRegistrations(string? name) => _name = name;

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<ManagedOptions>()
		         .Include(x => x.Dependencies.Recursive())
		         .Singleton()
		         //
		         .Then.AddSingleton<
			         ConfigureDistributedOutputs>(x => new(x.GetRequiredService<ManagedOptions>(), _name))
		         //
		         .AddStackExchangeRedisOutputCache(parameter.Deferred<ConfigureDistributedOutputs>().Assume());
	}
}