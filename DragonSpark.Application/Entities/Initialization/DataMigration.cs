using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Initialization;

/// <summary>
/// This is not very good, but the best we have for now:
/// https://github.com/dotnet/efcore/issues/24710#issuecomment-993242982
/// </summary>
public abstract class DataMigration<T> : Migration
	where T : DbContext
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
		if (services != null)
		{
			var       factory     = services.GetRequiredService<IDbContextFactory<T>>();
			var       initializer = services.GetRequiredService(_initializer).To<IInitializer>();
			using var context     = factory.CreateDbContext();

			Task.Run(initializer.Then().Bind(context).Then().Allocate()).GetAwaiter().GetResult();
		}
	}
}