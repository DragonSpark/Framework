using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore.Metadata;
using NetFabric.Hyperlinq;
using System.Buffers;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public sealed class MigrationOrder : IArray<IModel, IEntityType>
{
	public static MigrationOrder Default { get; } = new();

	MigrationOrder() : this(TopologicalSort.Default) {}
	
	readonly ITopologicalSort _sort;

	public MigrationOrder(ITopologicalSort sort) => _sort = sort;

	public Array<IEntityType> Get(IModel parameter)
	{
		using var entities = parameter.GetEntityTypes()
		                              .Where(t => !t.IsOwned() && t.FindPrimaryKey() != null)
		                              .AsValueEnumerable()
		                              .ToArray(ArrayPool<IEntityType>.Shared);
		var result = _sort.Get(entities).Open().Take(37).Result(); // TODO
		return result;
	}
}