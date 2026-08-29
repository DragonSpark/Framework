using Microsoft.EntityFrameworkCore.Metadata;
using NetFabric.Hyperlinq;
using System.Buffers;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

sealed class ComposeDependents : IDetermineDependents
{
	public static ComposeDependents Default { get; } = new();

	ComposeDependents() : this(Tarjan.Default) {}

	readonly ITarjan _tarjan;

	public ComposeDependents(ITarjan tarjan) => _tarjan = tarjan;

	public Dependents Get(Dictionary<IEntityType, List<IEntityType>> parameter)
	{
		var result = new Dependents();

		using var lease = _tarjan.Get(parameter)
		                         .AsValueEnumerable()
		                         .Select(g => g.ToList())
		                         .ToArray(ArrayPool<List<IEntityType>>.Shared);

		foreach (var g in lease)
		{
			result[g] = [];
			foreach (var type in g)
			{
				foreach (var dependencyGroup in parameter[type]
				                                .Select(x => lease.First(y => y.Contains(x)))
				                                .Where(y => y != g))
				{
					result[g].Add(dependencyGroup);
				}
			}
		}

		return result;
	}
}