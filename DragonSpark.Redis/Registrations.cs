using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Redis;

public sealed class Registrations<T> : ICommand<IServiceCollection> where T : ConfigureDistributedMemory
{
	public static Registrations<T> Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<T>()
		         .Include(x => x.Dependencies)
		         .Singleton()
		         //
		         .Then.AddStackExchangeRedisCache(parameter.Deferred<T>().Assume());
	}
}