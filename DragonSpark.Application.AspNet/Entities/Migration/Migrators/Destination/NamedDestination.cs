using DragonSpark.Application.AspNet.Entities.Migration.Steps;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

sealed class NamedDestination : IDestination<Dictionary<string, object>, Dictionary<string, object>>
{
	readonly IEntityType                 _type;
	readonly IStopAware<DbContext, bool> _first;

	public NamedDestination(IEntityType type) : this(type, FirstRun.Default) {}

	public NamedDestination(IEntityType type, IStopAware<DbContext, bool> first)
	{
		_type = type;
		_first  = first;
	}

	public async IAsyncEnumerable<Dictionary<string, object>> Get(
		Stop<DestinationInput<Dictionary<string, object>>> parameter)
	{
		var ((_, _, destination, from, _), stop) = parameter;

		if (await _first.Off(new(destination, stop)))
		{
			foreach (var item in from.Open())
			{
				var columns    = string.Join(", ", item.Keys.Select(k => $"[{k}]"));
				var parameters = string.Join(", ", item.Keys.Select((_, i) => $"@p{i}"));
				var values     = item.Values.ToArray();

				var schema = _type.GetSchema();
				var sql =
					$"INSERT INTO {(schema.IsAssigned() ? $"[{schema}]." : string.Empty)}[{_type.GetTableName()}] ({columns}) VALUES ({parameters})";

				await destination.Database.ExecuteSqlRawAsync(sql, values, stop).Off();
			}
		}

		yield break;
	}
}