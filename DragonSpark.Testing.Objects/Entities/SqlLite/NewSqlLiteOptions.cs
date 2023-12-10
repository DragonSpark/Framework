using DragonSpark.Application.Runtime;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Testing.Objects.Entities.Sql;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Testing.Objects.Entities.SqlLite;

public sealed class NewSqlLiteOptions<T> : SelectedResult<string, DbContextOptions<T>> where T : DbContext
{
	public static NewSqlLiteOptions<T> Default { get; } = new();

	NewSqlLiteOptions()
		: base(NewSqlDatabaseName.Default.Then().Bind(IdentifyingText.Default.Get), SqlLiteOptions<T>.Default.Get) {}
}