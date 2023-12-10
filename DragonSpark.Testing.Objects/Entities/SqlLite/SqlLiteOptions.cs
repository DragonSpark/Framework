using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Testing.Objects.Entities.SqlLite;

public sealed class SqlLiteOptions<T> : ISelect<string, DbContextOptions<T>> where T : DbContext
{
	public static SqlLiteOptions<T> Default { get; } = new();

	SqlLiteOptions() {}

	public DbContextOptions<T> Get(string parameter)
		=> new DbContextOptionsBuilder<T>().UseSqlite($"Data Source={parameter}").Options;
}