using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore.Metadata;
using NetFabric.Hyperlinq;
using System.Buffers;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public sealed class MigrationOrder : IArray<IModel, IEntityType>
{
	public static MigrationOrder Default { get; } = new();

	MigrationOrder() : this(TopologicalSort.Default) {}

	readonly ITopologicalSort        _sort;
	readonly Func<IEntityType, bool> _entity;

	public MigrationOrder(ITopologicalSort sort) : this(sort, MigrationEntity.Default.Get) {}

	public MigrationOrder(ITopologicalSort sort, Func<IEntityType, bool> entity)
	{
		_sort        = sort;
		_entity = entity;
	}

	public Array<IEntityType> Get(IModel parameter)
	{
		using var entities = parameter.GetEntityTypes()
									  .Where(_entity)
									  .AsValueEnumerable()
									  .ToArray(ArrayPool<IEntityType>.Shared);
		var result = _sort.Get(entities);
		return result;
	}
}