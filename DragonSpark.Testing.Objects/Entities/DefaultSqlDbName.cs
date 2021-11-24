namespace DragonSpark.Testing.Objects.Entities;

sealed class DefaultSqlDbName : Text.Text
{
	public static DefaultSqlDbName Default { get; } = new DefaultSqlDbName();

	DefaultSqlDbName() : base("temporary.efcore.testing.db") {}
}