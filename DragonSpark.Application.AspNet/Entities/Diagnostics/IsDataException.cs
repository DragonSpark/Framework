using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.Data.SqlClient;

namespace DragonSpark.Application.AspNet.Entities.Diagnostics;

public sealed class IsDataException : ICondition<Exception>
{
	public static IsDataException Default { get; } = new();

	IsDataException() : this(IsSqlDataException.Default.Get) {}

	readonly ISelect<Exception, SqlException?> _locate;
	readonly Func<SqlException, bool>          _predicate;

	public IsDataException(Func<SqlException, bool> predicate) : this(LocateDataException.Default, predicate) {}

	public IsDataException(ISelect<Exception, SqlException?> locate, Func<SqlException, bool> predicate)
	{
		_locate    = locate;
		_predicate = predicate;
	}

	public bool Get(Exception parameter) => _locate.Get(parameter) is {} data && _predicate(data);
}