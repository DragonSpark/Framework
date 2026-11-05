using DragonSpark.Application.AspNet.Entities.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.Data.SqlClient;
using Polly;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class PagePolicy : Result<IAsyncPolicy>
{
	public static PagePolicy Default { get; } = new();

	PagePolicy() : this(IsSqlDataException.Default.Then().Or(SevereCommandException.Default)) {}

	public PagePolicy(Func<SqlException, bool> handle)
		: base(new DurableDataConnectionPolicy(new IsDataException(handle).Get)) {}
}