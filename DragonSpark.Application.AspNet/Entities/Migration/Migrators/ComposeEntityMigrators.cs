using DragonSpark.Model.Selection;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class ComposeEntityMigrators : ISelect<IEnumerable<IEntityMigrator>, IEntityMigrator>
{
	public static ComposeEntityMigrators Default { get; } = new();

	ComposeEntityMigrators() {}

	public IEntityMigrator Get(IEnumerable<IEntityMigrator> parameter)
	{
		using var lease  = parameter.AsValueEnumerable().ToArray(ArrayPool<IEntityMigrator>.Shared);
		var result = lease.Length switch
		{
			0 => throw new InvalidOperationException("At least one IEntityMigrator is expected"), 
			1 => lease.Memory.Span[0],
			_ => new CompositeEntityMigrators(lease.Memory)
		};
		return result;
	}
}