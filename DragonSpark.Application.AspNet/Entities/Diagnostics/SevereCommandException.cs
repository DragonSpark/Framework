using DragonSpark.Model.Selection.Conditions;
using Microsoft.Data.SqlClient;

namespace DragonSpark.Application.AspNet.Entities.Diagnostics;

sealed class SevereCommandException : Condition<SqlException>
{
	public static SevereCommandException Default { get; } = new();

	SevereCommandException()
		: base(x => x.Message.StartsWith("A severe error occurred on the current command")) {}
}