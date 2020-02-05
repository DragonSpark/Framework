using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace DragonSpark.Server.Entities {
	public class StorageBuilder<T> : IDesignTimeDbContextFactory<T> where T : DbContext
	{
		readonly Action<DbContextOptionsBuilder> _configure;
		readonly Func<DbContextOptions<T>, T>    _create;

		public StorageBuilder() : this(ConfigureSqlServer<T>.Default.Execute, Start.A.Selection<DbContextOptions<T>>()
		                                                                           .AndOf<T>()
		                                                                           .By.Instantiation.Get()
		                                                                           .Get) {}

		public StorageBuilder(Action<DbContextOptionsBuilder> configure, Func<DbContextOptions<T>, T> create)
		{
			_configure = configure;
			_create    = create;
		}

		public T CreateDbContext(string[] args)
		{
			var builder = new DbContextOptionsBuilder<T>();
			_configure(builder);
			var result = _create(builder.Options);
			return result;
		}
	}
}