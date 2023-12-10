using DragonSpark.Application.Runtime;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Testing.Objects.Entities;

public sealed class NewSqlOptions<T> : SelectedResult<string, DbContextOptions<T>> where T : DbContext
{
	public static NewSqlOptions<T> Default { get; } = new();

	NewSqlOptions() : base(NewSqlDatabaseName.Default.Then().Bind(IdentifyingText.Default.Get),
	                       SqlOptions<T>.Default.Get) {}
}

// TODO
public sealed class NewSqlLiteOptions<T> : SelectedResult<string, DbContextOptions<T>> where T : DbContext
{
	public static NewSqlLiteOptions<T> Default { get; } = new();

	NewSqlLiteOptions()
		: base(NewSqlDatabaseName.Default.Then().Bind(IdentifyingText.Default.Get), SqlLiteOptions<T>.Default.Get) {}
}