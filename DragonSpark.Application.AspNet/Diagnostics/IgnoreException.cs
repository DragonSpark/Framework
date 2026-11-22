using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.JSInterop;

namespace DragonSpark.Application.AspNet.Diagnostics;

public sealed class IgnoreException : ICondition<Exception>
{
	public static IgnoreException Default { get; } = new();

	IgnoreException() : this(HasCancellationLanguage.Default) {}

	readonly ICondition<string> _message;

	public IgnoreException(ICondition<string> message) => _message = message;

	public bool Get(Exception parameter) => parameter switch
	{
		SqlException x => x.Number is 0 || _message.Get(x.Message),
		OperationCanceledException or JSDisconnectedException or NavigationException => true,
		_ => _message.Get(parameter.Message)
	};
}