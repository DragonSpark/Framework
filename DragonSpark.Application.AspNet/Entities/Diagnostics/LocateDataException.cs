using DragonSpark.Model.Selection;
using Microsoft.Data.SqlClient;

namespace DragonSpark.Application.AspNet.Entities.Diagnostics;

sealed class LocateDataException : ISelect<Exception, SqlException?>
{
	public static LocateDataException Default { get; } = new();

	LocateDataException() {}

	public SqlException? Get(Exception parameter)
	{
		var current = parameter;
		while (current is not null)
		{
			if (current is SqlException exception)
			{
				return exception;
			}

			current = current.InnerException;
		}

		return null;
	}
}