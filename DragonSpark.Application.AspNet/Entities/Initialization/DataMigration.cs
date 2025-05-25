using DragonSpark.Model.Results;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

[UsedImplicitly]
public abstract class DataMigration : Migration
{
	readonly IResult<IDataMigrationRegistry?> _registry;
	readonly ISeed                            _initializer;

	protected DataMigration(Type initializer)
		: this(LogicalMigrationRegistry.Default, new Seed(initializer)) {}

	protected DataMigration(IResult<IDataMigrationRegistry?> registry, ISeed initializer)
	{
		_registry    = registry;
		_initializer = initializer;
	}

	protected override void Up(MigrationBuilder migrationBuilder)
	{
		var registry = _registry.Get();
		registry?.Execute(_initializer);
	}

	protected override void Down(MigrationBuilder migrationBuilder) {}
}