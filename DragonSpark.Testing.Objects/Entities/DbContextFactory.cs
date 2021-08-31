using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Testing.Objects.Entities
{
	public class DbContextFactory<T> : Result<T>, IDbContextFactory<T> where T : DbContext
	{
		protected DbContextFactory(DbContextOptions<T> options)
			: this(Start.A.Selection<DbContextOptions>().By.Instantiation<T>().Bind(options)) {}

		protected DbContextFactory(Func<T> create) : base(create) {}

		public T CreateDbContext() => Get();
	}
}