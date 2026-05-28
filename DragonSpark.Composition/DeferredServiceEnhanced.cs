using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition;

sealed class DeferredServiceEnhanced<T> : Result<T> where T : notnull
{
	public DeferredServiceEnhanced(IServiceCollection collection) 
		: base(ServiceEnhanced<T>.Default.Then().Bind(collection.AsPopped()).Singleton()) {}
}