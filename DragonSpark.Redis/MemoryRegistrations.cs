using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Redis;

sealed class MemoryRegistrations<T> : ICommand<IServiceCollection> where T : ConfigureDistributedMemory
{
	public static MemoryRegistrations<T> Default { get; } = new();

	MemoryRegistrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<T>()
		         .Include(x => x.Dependencies)
		         .Singleton()
		         //
		         .Then.AddStackExchangeRedisCache(parameter.Deferred<T>().Assume());
	}
}

sealed class OutputsRegistrations<T> : ICommand<IServiceCollection> where T : ConfigureDistributedOutputs
{
	public static OutputsRegistrations<T> Default { get; } = new();

	OutputsRegistrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<T>()
		         .Include(x => x.Dependencies)
		         .Singleton()
		         //
		         .Then.AddStackExchangeRedisOutputCache(parameter.Deferred<T>().Assume());
	}
}