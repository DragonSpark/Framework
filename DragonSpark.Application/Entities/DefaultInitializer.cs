using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities {
	sealed class DefaultInitializer<T> : IInitializer<T> where T : DbContext
	{
		public static DefaultInitializer<T> Default { get; } = new DefaultInitializer<T>();

		DefaultInitializer() {}

		public T Get(T parameter) => parameter.With(x => x.Database.EnsureCreated());
	}
}