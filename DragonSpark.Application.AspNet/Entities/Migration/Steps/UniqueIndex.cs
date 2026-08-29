namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

public sealed record UniqueIndex(
	string SchemaName,
	string TableName,
	string IndexName,
	bool IsUniqueConstraint,
	byte KeyOrdinal,
	bool IsDescending,
	bool IsIncluded,
	string ColumnName,
	string? FilterDefinition
);