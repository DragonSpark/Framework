using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NetFabric.Hyperlinq;
using System.Buffers;

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
									  .Where(t => t.IsOwned() && t.GetViewName() is null
												  && t.FindPrimaryKey() != null)
									  .AsValueEnumerable()
									  .ToArray(ArrayPool<IEntityType>.Shared);
		var result = _sort.Get(entities);
		return result;
	}

	// TODO:
	/*public Array<IEntityType> Get(IModel parameter)
	{
		using var entities = parameter.GetEntityTypes()
		                              .Where(t => t.Name.EndsWith(".IssuanceProcess")
		                                          && !t.IsOwned() && t.GetViewName() is null
		                                          && t.FindPrimaryKey() != null)
		                              .AsValueEnumerable()
		                              .ToArray(ArrayPool<IEntityType>.Shared);
		var result = _sort.Get(entities);
		return result;
	}*/

	/*public Array<IEntityType> Get(IModel parameter)
	{
		using var entities = parameter.GetEntityTypes()
		                              .Where(t => (t.Name.EndsWith(".IssuanceProcess")
		                                           || t.Name.EndsWith(".StandardUserAccount")
		                                           || t.Name.EndsWith(".ProductDefinition")
		                                           || t.Name.EndsWith(".MarketplaceProfile")
		                                           || t.Name.EndsWith(".ImageFileDetails")
		                                           || t.Name.EndsWith(".DocumentFileDetails")
		                                           || t.Name.EndsWith(".AudioFileDetails")
		                                           || t.Name.EndsWith(".VideoFileDetails")
		                                           || t.Name.EndsWith(".TextFileDetails")
		                                           || t.Name.EndsWith(".Identity.User"))
		                                          && !t.IsOwned() && t.GetViewName() is null
		                                          && t.FindPrimaryKey() != null)
		                              .AsValueEnumerable()
		                              .ToArray(ArrayPool<IEntityType>.Shared);
		var result = entities.ToArray();
		return result;
	}*/
}