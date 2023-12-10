namespace DragonSpark.Testing.Objects.Entities.Sql;

sealed class DefaultSqlDbName : Text.Text
{
	public static DefaultSqlDbName Default { get; } = new();

	DefaultSqlDbName() : base("temporary.efcore.testing.db") {}
}