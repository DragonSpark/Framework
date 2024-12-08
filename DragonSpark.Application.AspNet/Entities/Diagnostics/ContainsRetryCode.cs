using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.Data.SqlClient;

namespace DragonSpark.Application.AspNet.Entities.Diagnostics;

sealed class ContainsRetryCode : Condition<SqlException>
{
	public static ContainsRetryCode Default { get; } = new();

	ContainsRetryCode()
		: base(Start.A.Selection<SqlException>().By.Calling(x => x.Number).Select(RetryCodes.Default)) {}
}