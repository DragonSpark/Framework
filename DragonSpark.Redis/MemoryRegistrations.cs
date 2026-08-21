using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Redis;

sealed class MemoryRegistrations : ICommand<IServiceCollection>
{
	readonly string? _name;

	public MemoryRegistrations(string? name) => _name = name;

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<ManagedOptions>()
		         .Include(x => x.Dependencies)
		         .Singleton()
		         .Then.AddSingleton<ConfigureDistributedMemory>(x => new(x.GetRequiredService<ManagedOptions>(), _name))
		         //
		         .AddStackExchangeRedisCache(parameter.Deferred<ConfigureDistributedMemory>().Assume());
	}
}