using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Testing.Objects.Entities.SqlLite;

public sealed class SqlLiteDbContexts<T> : DbContextFactory<T> where T : DbContext
{
	public SqlLiteDbContexts() : this(Guid.NewGuid().ToString()) {}

	public SqlLiteDbContexts(string name) : this(SqlLiteOptions<T>.Default.Get(name)) {}

	public SqlLiteDbContexts(DbContextOptions<T> options) : base(options) {}
}