using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Results;
using DragonSpark.Runtime.Execution;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

/// <summary>
/// This is not very good, but the best we have for now:
/// https://github.com/dotnet/efcore/issues/24710#issuecomment-993242982
/// </summary>
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

sealed class LogicalMigrationRegistry : Logical<IDataMigrationRegistry>
{
	public static LogicalMigrationRegistry Default { get; } = new();

	LogicalMigrationRegistry() {}
}

public interface IDataMigrationRegistry : ICommand<ISeed>, IOperation<DbContext>;

sealed class DataMigrationRegistry : IDataMigrationRegistry
{
	readonly IServiceProvider _services;
	readonly HashSet<ISeed>   _initializers;

	public DataMigrationRegistry(IServiceProvider services) : this(services, new()) {}

	public DataMigrationRegistry(IServiceProvider services, HashSet<ISeed> initializers)
	{
		_services     = services;
		_initializers = initializers;
	}

	public void Execute(ISeed parameter)
	{
		_initializers.Add(parameter);
	}

	public async ValueTask Get(DbContext parameter)
	{
		foreach (var initializer in _initializers)
		{
			await initializer.Off(new(_services, parameter));
		}
	}
}

public sealed class ApplyMigrationRegistry : IAllocated<DbContext>
{
	public static ApplyMigrationRegistry Default { get; } = new();

	ApplyMigrationRegistry() : this(LogicalMigrationRegistry.Default) {}

	readonly IResult<IDataMigrationRegistry?> _registry;

	public ApplyMigrationRegistry(IResult<IDataMigrationRegistry?> registry) => _registry = registry;

	public Task Get(DbContext parameter) => _registry.Get().Verify("Migration registry not found").Allocate(parameter);

	public Task Get(DbContext parameter, bool seeded, CancellationToken _) => Get(parameter);
}