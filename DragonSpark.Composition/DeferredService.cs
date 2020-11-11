using DragonSpark.Model.Results;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition
{
	sealed class DeferredService<T> : IResult<T> where T : notnull
	{
		readonly IServiceCollection _collection;

		public DeferredService(IServiceCollection collection) => _collection = collection;

		public T Get() => _collection.BuildServiceProvider(false).GetRequiredService<T>();
	}
}