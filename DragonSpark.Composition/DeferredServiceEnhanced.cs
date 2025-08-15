using DragonSpark.Model.Results;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition;

sealed class DeferredServiceEnhanced<T> : FixedSelection<IServiceCollection, T> where T : notnull
{
    public DeferredServiceEnhanced(IServiceCollection collection) : base(ServiceEnhanced<T>.Default, collection) {}
}