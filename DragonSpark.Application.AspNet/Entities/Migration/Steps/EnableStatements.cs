using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

sealed class EnableStatements : ISelect<ConstraintInput, IEnumerable<string>>
{
	public static EnableStatements Default { get; } = new();

	EnableStatements() {}

	public IEnumerable<string> Get(ConstraintInput parameter)
	{
		var (targets, indexes) = parameter;
		foreach (var (schema, table, column) in targets.Open())
		{
			yield return $"ALTER TABLE [{schema}].[{table}] DROP COLUMN [{column}]";
			yield return $"ALTER TABLE [{schema}].[{table}] ADD [{column}] ROWVERSION";
		}

		// 4. Recreate all unique indexes + constraints
		foreach (var group in indexes.Open())
		{
			var first = group.First();
			var (schema, table, indexName) = group.Key;

			var columns = string.Join(", ",
			                          group.Where(i => i.KeyOrdinal > 0)
			                               .OrderBy(i => i.KeyOrdinal)
			                               .Select(i => $"[{i.ColumnName}] {(i.IsDescending ? "DESC" : "ASC")}"));

			var included      = group.Where(i => i.IsIncluded).Select(i => $"[{i.ColumnName}]").ToList();
			var includeClause = included.Any() ? $" INCLUDE ({string.Join(", ", included)})" : string.Empty;
			var filter = first.FilterDefinition is { Length: > 0 }
				             ? $" WHERE {first.FilterDefinition}"
				             : string.Empty;

			yield return first.IsUniqueConstraint
				             ? $"ALTER TABLE [{schema}].[{table}] ADD CONSTRAINT [{indexName}] UNIQUE ({columns})"
				             : $"CREATE UNIQUE INDEX [{indexName}] ON [{schema}].[{table}] ({columns}){includeClause}{filter}";
		}

		yield return "EXEC sp_msforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';";
	}
}