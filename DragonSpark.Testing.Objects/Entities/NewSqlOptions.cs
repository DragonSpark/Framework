using DragonSpark.Application.Runtime;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Testing.Objects.Entities;

public sealed class NewSqlOptions<T> : DelegatedSelection<string, DbContextOptions<T>> where T : DbContext
{
	public static NewSqlOptions<T> Default { get; } = new();

	NewSqlOptions() : base(SqlOptions<T>.Default.Get,
	                       NewSqlDatabaseName.Default.Then().Bind(IdentifyingText.Default.Get)) {}
}