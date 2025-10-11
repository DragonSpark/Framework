using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Redis;

sealed class OutputsRegistrations<T> : ICommand<IServiceCollection> where T : ConfigureDistributedOutputs
{
	public static OutputsRegistrations<T> Default { get; } = new();

	OutputsRegistrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<T>()
		         .Include(x => x.Dependencies.Recursive())
		         .Singleton()
		         //
		         .Then.AddStackExchangeRedisOutputCache(parameter.Deferred<T>().Assume());
	}
}