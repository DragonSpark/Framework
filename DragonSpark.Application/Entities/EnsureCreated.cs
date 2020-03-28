using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities
{
	public sealed class EnsureCreated<T> : IInitializer<T> where T : DbContext
	{
		[UsedImplicitly]
		public static EnsureCreated<T> Default { get; } = new EnsureCreated<T>();

		EnsureCreated() {}

		public T Get(T parameter) => parameter.With(x => x.Database.EnsureCreated());
	}
}