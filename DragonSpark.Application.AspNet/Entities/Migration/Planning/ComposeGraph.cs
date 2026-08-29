using Microsoft.EntityFrameworkCore.Metadata;
using NetFabric.Hyperlinq;
using System.Buffers;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

sealed class ComposeGraph : IComposeGraph
{
	public static ComposeGraph Default { get; } = new();

	ComposeGraph() : this(Dependencies.Default.Get) {}

	readonly Func<IEntityType, List<IEntityType>> _dependencies;

	public ComposeGraph(Func<IEntityType, List<IEntityType>> dependencies) => _dependencies = dependencies;

	public Dictionary<IEntityType, List<IEntityType>> Get(Lease<IEntityType> parameter)
	{
		var result = parameter.ToDictionary(x => x, _dependencies);

		foreach (var key in result.Keys)
		{
			var       types = result[key];
			using var lease = types.AsValueEnumerable().ToArray(ArrayPool<IEntityType>.Shared);
			foreach (var type in lease)
			{
				if (!result.ContainsKey(type))
				{
					types.Remove(type);
				}
			}
		}

		return result;
	}
}