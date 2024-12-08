using DragonSpark.Model.Selection.Conditions;
using Microsoft.Data.SqlClient;

namespace DragonSpark.Application.AspNet.Entities.Diagnostics;

sealed class NetworkRelatedException : Condition<SqlException>
{
	public static NetworkRelatedException Default { get; } = new();

	NetworkRelatedException()
		: base(x => x.Number == -1 &&
		            x.Message
		             .StartsWith("A network-related or instance-specific error occurred while establishing a connection to SQL Server")) {}
}