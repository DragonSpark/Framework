using Microsoft.EntityFrameworkCore.Metadata;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

sealed class ComposeGraph : IComposeGraph
{
	public static ComposeGraph Default { get; } = new();

	ComposeGraph() : this(Dependencies.Default.Get) {}

	readonly Func<IEntityType, HashSet<IEntityType>> _dependencies;

	public ComposeGraph(Func<IEntityType, HashSet<IEntityType>> dependencies) => _dependencies = dependencies;

	public Dictionary<IEntityType, HashSet<IEntityType>> Get(Lease<IEntityType> parameter)
	{
		var result = parameter.ToDictionary(x => x, _dependencies);

		foreach (var key in result.Keys)
		{
			result[key].RemoveWhere(x => !result.ContainsKey(x));
		}

		return result;
	}
}