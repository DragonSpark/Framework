using DragonSpark.Model.Selection.Conditions;
using Microsoft.Data.SqlClient;
using Microsoft.JSInterop;
using System;

namespace DragonSpark.Presentation.Components.Diagnostics;

sealed class IgnoreException : ICondition<Exception>
{
	public static IgnoreException Default { get; } = new();

	IgnoreException() {}

	public bool Get(Exception parameter) => parameter switch
	{
		SqlException x => x.Message.Contains("Operation cancelled by user."),
		OperationCanceledException => true,
		JSDisconnectedException => true,
		_ => false
	};
}