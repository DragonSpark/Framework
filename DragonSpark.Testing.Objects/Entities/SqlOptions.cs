using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Testing.Objects.Entities;

public sealed class SqlOptions<T> : ISelect<string, DbContextOptions<T>> where T : DbContext
{
	public static SqlOptions<T> Default { get; } = new();

	SqlOptions() {}

	public DbContextOptions<T> Get(string parameter)
	{
		var connection =
			$@"Server=(localdb)\mssqllocaldb;Database={parameter};Trusted_Connection=True";
		var result = new DbContextOptionsBuilder<T>().UseSqlServer(connection).Options;
		return result;
	}
}

// TODO

public sealed class SqlLiteOptions<T> : ISelect<string, DbContextOptions<T>> where T : DbContext
{
	public static SqlLiteOptions<T> Default { get; } = new();

	SqlLiteOptions() {}

	public DbContextOptions<T> Get(string parameter)
		=> new DbContextOptionsBuilder<T>().UseSqlite($"Data Source={parameter}").Options;
}