using DragonSpark.Model.Selection.Conditions;
using Microsoft.Data.SqlClient;
using Microsoft.JSInterop;

namespace DragonSpark.Application.AspNet.Diagnostics;

public sealed class IgnoreException : ICondition<Exception>
{
	public static IgnoreException Default { get; } = new();

	IgnoreException() : this("Operation cancelled by user.") {}

	readonly string _message;

	public IgnoreException(string message) => _message = message;

	public bool Get(Exception parameter) => parameter switch
	{
		SqlException x => x.Number == 0 || x.Message.Contains(_message),
		InvalidOperationException x => x.Message.Contains(_message),
		OperationCanceledException => true,
		JSDisconnectedException => true,
		_ => false
	};
}