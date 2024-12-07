using DragonSpark.Compose;
using DragonSpark.Model.Results;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Initialization;

/// <summary>
/// This is not very good, but the best we have for now:
/// https://github.com/dotnet/efcore/issues/24710#issuecomment-993242982
/// </summary>
[UsedImplicitly]
public abstract class DataMigration<T> : Migration where T : DbContext
{
	readonly IResult<IServiceProvider?> _services;
	readonly Type                       _initializer;

	protected DataMigration(Type initializer) : this(CurrentServices.Default, initializer) {}

	protected DataMigration(IResult<IServiceProvider?> services, Type initializer)
	{
		_services    = services;
		_initializer = initializer;
	}

	protected override void Up(MigrationBuilder migrationBuilder)
	{
		var services = _services.Get();
		if (services is not null)
		{
			Task.Run(new Transaction<T>(services, _initializer).Then().Allocate()).GetAwaiter().GetResult();
		}
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		//base.Down(migrationBuilder);
	}
}