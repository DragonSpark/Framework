using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Testing.Objects.Entities;

public sealed class SqlDbContexts<T> : DbContextFactory<T> where T : DbContext
{
	public SqlDbContexts(string name) : this(SqlOptions<T>.Default.Get(name)) {}

	public SqlDbContexts(DbContextOptions<T> options) : base(options) {}
}

// TODO

public sealed class SqlLiteDbContexts<T> : DbContextFactory<T> where T : DbContext
{
	public SqlLiteDbContexts() : this(Guid.NewGuid().ToString()) {}

	public SqlLiteDbContexts(string name) : this(SqlLiteOptions<T>.Default.Get(name)) {}

	public SqlLiteDbContexts(DbContextOptions<T> options) : base(options) {}
}