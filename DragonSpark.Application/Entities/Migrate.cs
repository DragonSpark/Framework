using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	public sealed class Migrate<T> : IInitializer<T> where T : DbContext
	{
		[UsedImplicitly]
		public static Migrate<T> Default { get; } = new Migrate<T>();

		Migrate() {}

		public async ValueTask Get(T parameter)
		{
			await parameter.Database.MigrateAsync().ConfigureAwait(false);
		}
	}
}