using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Testing.Objects.Entities
{
	public sealed class SqlDbContexts<T> : DbContextFactory<T> where T : DbContext
	{
		public SqlDbContexts() : this(DefaultSqlDbName.Default) {}

		public SqlDbContexts(string name)
			: this(new DbContextOptionsBuilder<T>()
			       .UseSqlServer($@"Server=(localdb)\mssqllocaldb;Database={name};Trusted_Connection=True;MultipleActiveResultSets=true")
			       .Options) {}

		public SqlDbContexts(DbContextOptions<T> options) : base(options) {}
	}
}