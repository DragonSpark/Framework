using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	public sealed class EnsureCreated<T> : IInitializer<T> where T : DbContext
	{
		[UsedImplicitly]
		public static EnsureCreated<T> Default { get; } = new EnsureCreated<T>();

		EnsureCreated() {}

		public async ValueTask Get(T parameter)
		{
			await parameter.Database.EnsureCreatedAsync().ConfigureAwait(false);
		}
	}
}