using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore.Metadata;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

sealed class TopologicalSort : ArraySelection<Lease<IEntityType>, IEntityType>, ITopologicalSort
{
	public static TopologicalSort Default { get; } = new();

	TopologicalSort() : this(ComposeGraph.Default, ComposeDependents.Default, ComposeEntities.Default) {}

	public TopologicalSort(IComposeGraph graph, IDetermineDependents dependents,
	                       IArray<Dependents, IEntityType> entities)
		: base(graph.Then().Select(dependents).Select(entities)) {}
}