using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore.Metadata;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

sealed class ComposeEntities : IArray<Dependents, IEntityType>
{
	public static ComposeEntities Default { get; } = new();

	ComposeEntities() : this(x => !x.IsAbstract()) {}

	readonly Func<IEntityType, bool> _where;

	public ComposeEntities(Func<IEntityType, bool> where) => _where = where;

	public Array<IEntityType> Get(Dependents parameter)
	{
		var entities  = new Entities();
		var remaining = parameter.ToDictionary(x => x.Key, x => x.Value.ToHashSet());
		while (true)
		{
			using var ready = remaining.AsValueEnumerable()
			                           .Where(x => x.Value.Count == 0) // no dependencies
			                           .Select(x => x.Key)
			                           .ToArray(ArrayPool<List<IEntityType>>.Shared);

			if (ready.Length == 0)
			{
				break;
			}

			foreach (var r in ready)
			{
				entities.Add(r);
				remaining.Remove(r);

				foreach (var d in remaining.Values)
				{
					d.Remove(r);
				}
			}
		}

		entities.AddRange(remaining.Keys);

		var result = entities.SelectMany(x => x.AsEnumerable().Reverse()).Where(_where).Result();
		return result;
	}
}