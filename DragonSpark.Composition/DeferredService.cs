using DragonSpark.Model.Results;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition;

sealed class DeferredService<T> : FixedSelection<IServiceCollection, T> where T : notnull
{
	public DeferredService(IServiceCollection collection) : base(Service<T>.Default, collection) {}
}