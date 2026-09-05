using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

sealed class NamedDestination : IDestination<Dictionary<string, object>, Dictionary<string, object>>
{
	readonly IEntityType _type;

	public NamedDestination(IEntityType type) => _type = type;

	public async IAsyncEnumerable<Dictionary<string, object>> Get(
		Stop<DestinationInput<Dictionary<string, object>>> parameter)
	{
		var ((_, _, destination, from, _), stop) = parameter;

		foreach (var item in from.Open())
		{
			var keys       = item.Keys.ToArray();
			var columns    = string.Join(", ", keys.Select(k => $"[{k}]"));
			var parameters = string.Join(", ", keys.Select((_, i) => $"@p{i}"));
			var values     = item.Values.ToArray();
			var match      = string.Join(" AND ", keys.Select((k, i) => $"[{k}] = @p{i}"));
			var schema     = _type.GetSchema();
			var tableName  = (schema.IsAssigned() ? $"[{schema}]." : string.Empty) + $"[{_type.GetTableName()}]";

			var sql = $"""
			           	INSERT INTO {tableName} ({columns})
			           	SELECT {parameters}
			           	WHERE NOT EXISTS (
			           		SELECT 1 FROM {tableName} WHERE {match}
			           	);
			           """;

			await destination.Database.ExecuteSqlRawAsync(sql, values, stop).Off();
		}

		yield break;
	}
}