namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

sealed class IndexesQuery : Text.Text
{
	public static IndexesQuery Default { get; } = new();

	IndexesQuery()
		: base("""
		       SELECT
		           s.name AS SchemaName,
		           t.name AS TableName,
		           i.name AS IndexName,
		           i.is_unique_constraint AS IsUniqueConstraint,
		           ic.key_ordinal AS KeyOrdinal,
		           ic.is_descending_key AS IsDescending,
		           ic.is_included_column AS IsIncluded,
		           col.name AS ColumnName,
		           i.filter_definition AS FilterDefinition
		       FROM sys.indexes i
		       JOIN sys.index_columns ic 
		           ON ic.object_id = i.object_id 
		          AND ic.index_id = i.index_id
		       JOIN sys.columns col 
		           ON col.object_id = ic.object_id 
		          AND col.column_id = ic.column_id
		       JOIN sys.tables t 
		           ON t.object_id = i.object_id
		       JOIN sys.schemas s 
		           ON s.schema_id = t.schema_id
		       WHERE i.is_unique = 1
		         AND i.is_primary_key = 0
		       ORDER BY s.name, t.name, i.name, ic.key_ordinal;
		       """) {}
}