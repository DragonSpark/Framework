using DragonSpark.Application.Compose.Entities;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace DragonSpark.Application.Entities.Design
{
	public class StorageBuilder<T> : IResult<T>, IDesignTimeDbContextFactory<T> where T : DbContext
	{
		readonly Action<DbContextOptionsBuilder<T>> _configure;
		readonly Func<DbContextOptions<T>, T>       _create;

		protected StorageBuilder() : this(ConfigureSqlServer<T>.Default.Execute) {}

		protected StorageBuilder(Action<DbContextOptionsBuilder<T>> configure)
			: this(configure, Start.A.Selection<DbContextOptions<T>>()
			                       .AndOf<T>()
			                       .By.Instantiation.Get()
			                       .Get) {}

		protected StorageBuilder(Func<DbContextOptions<T>, T> create)
			: this(ConfigureSqlServer<T>.Default.Execute, create) {}

		protected StorageBuilder(Action<DbContextOptionsBuilder<T>> configure, Func<DbContextOptions<T>, T> create)
		{
			_configure = configure;
			_create    = create;
		}

		T IDesignTimeDbContextFactory<T>.CreateDbContext(string[] args) => Get();

		public T Get()
		{
			var builder = new DbContextOptionsBuilder<T>();
			_configure(builder);
			var result = _create(builder.Options);
			return result;
		}
	}
}