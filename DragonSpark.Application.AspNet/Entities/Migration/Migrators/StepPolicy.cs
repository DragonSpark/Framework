using DragonSpark.Application.AspNet.Entities.Diagnostics;
using DragonSpark.Model.Results;
using Microsoft.Data.SqlClient;
using Polly;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class StepPolicy : Result<IAsyncPolicy>
{
	public static StepPolicy Default { get; } = new();

	StepPolicy() : this(SevereCommandException.Default.Get) {}

	public StepPolicy(Func<SqlException, bool> handle) 
		: base(new DurableDataConnectionPolicy(new IsDataException(handle).Get)) {}
}