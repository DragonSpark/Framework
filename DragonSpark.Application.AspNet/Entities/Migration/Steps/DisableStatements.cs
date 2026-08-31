using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

sealed class DisableStatements : ISelect<ConstraintInput, IEnumerable<string>>
{
	public static DisableStatements Default { get; } = new();

	DisableStatements() {}

	public IEnumerable<string> Get(ConstraintInput parameter)
	{
		var (targets, indexes) = parameter;
		yield return "EXEC sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';";
		foreach (var group in indexes.Open())
		{
			var (schema, table, indexName) = group.Key;
			var first = group.First();

			yield return first.IsUniqueConstraint
				             ? $"ALTER TABLE [{schema}].[{table}] DROP CONSTRAINT [{indexName}]"
				             : $"DROP INDEX [{indexName}] ON [{schema}].[{table}]";
		}

		foreach (var (schema, table, column) in targets.Open())
		{
			yield return $"ALTER TABLE [{schema}].[{table}] DROP COLUMN [{column}]";
			yield return $"ALTER TABLE [{schema}].[{table}] ADD [{column}] VARBINARY(8) NULL";
			yield return
				$"UPDATE [{schema}].[{table}] SET [{column}] = 0x0000000000000000 WHERE [{column}] IS NULL";
		}
	}
}