using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Testing.Objects.Entities
{
	public sealed class MemoryOptions<T> : ISelect<string, DbContextOptions<T>> where T : DbContext
	{
		public static MemoryOptions<T> Default { get; } = new MemoryOptions<T>();

		MemoryOptions() {}

		public DbContextOptions<T> Get(string parameter)
			=> new DbContextOptionsBuilder<T>().UseInMemoryDatabase(parameter).Options;
	}
}