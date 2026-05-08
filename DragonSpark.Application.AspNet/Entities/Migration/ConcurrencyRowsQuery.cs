namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class ConcurrencyRowsQuery : Text.Text
{
	public static ConcurrencyRowsQuery Default { get; } = new();

	ConcurrencyRowsQuery() : base(@"SELECT 
		s.name AS [Schema], 
		t.name AS [Table], 
		c.name AS [Column]
	FROM sys.columns c
	JOIN sys.tables t ON c.object_id = t.object_id
	JOIN sys.schemas s ON t.schema_id = s.schema_id
	WHERE c.system_type_id = 189") {}
}