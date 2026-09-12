using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using DragonSpark.Application.AspNet.Entities.Migration.Steps;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public class Migration : IMigration
{
	readonly EntityMigratorInput   _input;
	readonly Array<IMigrationStep> _steps;

	// ReSharper disable once TooManyDependencies
	protected Migration(ILogger logger, IWorkspaceDefinition definition, IEntityMigrators processors,
	                    IMigrationSteps steps)
		: this(logger, definition, steps, processors.Get(definition)) {}

	// ReSharper disable once TooManyDependencies
	protected Migration(ILogger logger, IWorkspaceDefinition definition, IMigrationSteps steps,
	                    params IEntityMigrator[] migrators)
		: this(logger, definition, [.. steps.Get(migrators)]) {}

	protected Migration(ILogger logger, IWorkspaceDefinition workspaces, params IMigrationStep[] steps)
		: this(new(logger, workspaces, DefaultBatchSize.Default), steps) {}

	protected Migration(EntityMigratorInput input, params IMigrationStep[] steps)
	{
		_input = input;
		_steps = steps;
	}

	public async ValueTask Get(Stop<ushort> parameter)
	{
		var (subject, stop) = parameter;
		var updated = _input with { BatchSize = subject };
		var input   = updated.Stop(stop);
		foreach (var step in _steps.Open())
		{
			await step.Off(input);
		}
	}

	public ValueTask Get(CancellationToken parameter) => Get(new(_input.BatchSize, parameter));
}