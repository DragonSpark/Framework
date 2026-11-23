using DragonSpark.Model.Selection.Conditions;
using Microsoft.Data.SqlClient;

namespace DragonSpark.Application.AspNet.Entities.Diagnostics;

public sealed class IsSqlDataException : AnyCondition<SqlException>
{
	public static IsSqlDataException Default { get; } = new();

	IsSqlDataException() : base(ContainsRetryCode.Default, NetworkRelatedException.Default) {}
}