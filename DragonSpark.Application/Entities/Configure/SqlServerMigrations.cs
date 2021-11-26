using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragonSpark.Application.Entities.Configure;

public sealed class SqlServerMigrations : ISqlServerConfiguration
{
	readonly string _name;

	public SqlServerMigrations(string name) => _name = name;

	public void Execute(SqlServerDbContextOptionsBuilder parameter)
	{
		parameter.MigrationsAssembly(_name);
	}
}