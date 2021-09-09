using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Testing.Objects.Entities
{
	public sealed class SqlDbContexts<T> : DbContextFactory<T> where T : DbContext
	{
		public SqlDbContexts(string name) : this(SqlOptions<T>.Default.Get(name)) {}

		public SqlDbContexts(DbContextOptions<T> options) : base(options) {}
	}
}