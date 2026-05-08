using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using NetFabric.Hyperlinq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class DefaultMap : IMap
{
	readonly IMap _previous;
	public static DefaultMap Default { get; } = new();

	DefaultMap() : this(Map.Default) {}

	public DefaultMap(IMap previous) => _previous = previous;

	public ValueTask Get(Stop<MapInput> parameter)
	{
		var ((from, to), _) = parameter;

		if (from.Entity is Dictionary<string, object> source && to.Entity is Dictionary<string, object> destination)
		{
			foreach (var key in source.Keys)
			{
				destination[key] = source[key];
			}

			var exists = to.Context.Set<Dictionary<string, object>>(to.Metadata.Name)
			               .AsNoTracking()
			               .AsEnumerable()
			               .Any(row => source.All(kvp => row.ContainsKey(kvp.Key) &&
			                                             Equals(row[kvp.Key], kvp.Value)));

			to.State = exists ? EntityState.Detached : EntityState.Added;

			return ValueTask.CompletedTask;
		}

		return _previous.Get(parameter);
	}
}