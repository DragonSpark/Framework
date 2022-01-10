using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace DragonSpark.Application.Entities.Configure;

public sealed class SqlServerMigrations : ISqlServerConfiguration
{
	readonly string _name;

	public SqlServerMigrations(Type reference) : this(reference.Assembly.GetName().Name.Verify()) {}

	public SqlServerMigrations(string name) => _name = name;

	public void Execute(SqlServerDbContextOptionsBuilder parameter)
	{
		parameter.MigrationsAssembly(_name);
	}
}