using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Testing.Objects.Entities.Sql;

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