using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities
{
	sealed class StorageInitializer<T> : IStorageInitializer<T> where T : DbContext
	{
		readonly Array<IInitializer<T>> _initializers;

		public StorageInitializer(params IInitializer<T>[] initializers) => _initializers = initializers;

		public T Get(T parameter) => _initializers.Open().Alter(parameter);
	}
}