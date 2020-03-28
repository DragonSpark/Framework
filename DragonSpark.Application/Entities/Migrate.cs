using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities
{
	public sealed class Migrate<T> : IInitializer<T> where T : DbContext
	{
		[UsedImplicitly]
		public static Migrate<T> Default { get; } = new Migrate<T>();

		Migrate() {}

		public T Get(T parameter)
		{
			parameter.Database.Migrate();
			return parameter;
		}
	}
}