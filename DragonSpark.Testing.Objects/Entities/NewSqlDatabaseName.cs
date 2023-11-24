using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Testing.Objects.Entities;

public sealed class NewSqlDatabaseName : IAlteration<string>
{
	public static NewSqlDatabaseName Default { get; } = new();

	NewSqlDatabaseName() : this(DefaultSqlDbName.Default) {}

	readonly string _base;

	public NewSqlDatabaseName(string @base) => _base = @base;

	public string Get(string parameter) => $"{_base}-{parameter}";
}