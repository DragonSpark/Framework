using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition;

sealed class DeferredService<T> : Result<T> where T : notnull
{
	public DeferredService(IServiceCollection collection) 
		: base(Service<T>.Default.Then().Bind(collection.AsPopped()).Singleton()) {}
}