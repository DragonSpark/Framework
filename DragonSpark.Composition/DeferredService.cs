using DragonSpark.Model.Results;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition;

sealed class DeferredService<T> : FixedSelection<IServiceCollection, T> where T : notnull // TODO: Need to store and pop the reference to collection after resolution instead so that it's not captured in memory
{
	public DeferredService(IServiceCollection collection) : base(Service<T>.Default, collection) {}
}