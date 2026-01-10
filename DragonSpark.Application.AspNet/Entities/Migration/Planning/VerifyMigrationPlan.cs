using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public sealed class VerifyMigrationPlan : ISelect<IReadOnlyCollection<IEntityType>, IReadOnlyCollection<string>>
{
	readonly ICommand<VerifyMigrationStepInput> _step;
	public static VerifyMigrationPlan Default { get; } = new();

	VerifyMigrationPlan() : this(VerifyMigrationStep.Default) {}

	public VerifyMigrationPlan(ICommand<VerifyMigrationStepInput> step) => _step = step;

	public IReadOnlyCollection<string> Get(IReadOnlyCollection<IEntityType> parameter)
	{
		using var lease  = parameter.AsValueEnumerable().ToArray(ArrayPool<IEntityType>.Shared);
		var       span   = lease.Memory.Span;
		var       result = new List<string>();
		for (var i = 0; i < lease.Length; i++)
		{
			var current = span[i];

			foreach (var fk in current.GetForeignKeys().Where(f => f.IsRequired))
			{
				_step.Execute(new(fk, lease.Memory, result, (uint)i, current));
			}
		}

		return result;
	}
}

public readonly record struct VerifyMigrationStepInput(
	IForeignKey Key,
	ReadOnlyMemory<IEntityType> Types,
	List<string> Result,
	uint index,
	IEntityType Current);

sealed class VerifyMigrationStep : ICommand<VerifyMigrationStepInput>
{
	public static VerifyMigrationStep Default { get; } = new();

	VerifyMigrationStep() {}

	public void Execute(VerifyMigrationStepInput parameter)
	{
		var (key, types, result, i, current) = parameter;
		var principal = key.PrincipalEntityType;
		using var lease = principal.GetDerivedTypes()
		                           .AsValueEnumerable()
		                           .ToArray(ArrayPool<IEntityType>.Shared);
		var checkTypes = lease.Any() ? lease.Where(d => !d.ClrType.IsAbstract) : [principal];

		foreach (var check in checkTypes)
		{
			var index = types.Span.IndexOf(check);
			if (index == -1)
			{
				result.Add($"[{i}] {current.GetTableName()} depends on missing {check.GetTableName()}");
			}
			else if (index >= i)
			{
				result.Add($"[{i}] {current.GetTableName()} depends on later [{index}] {check.GetTableName()} — VIOLATION RISK");
			}
		}

	}
}