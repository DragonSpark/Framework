using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.Data.SqlClient;

namespace DragonSpark.Application.AspNet.Entities.Diagnostics;

sealed class IsDataException : ICondition<Exception>
{
	public static IsDataException Default { get; } = new();

	IsDataException()
		: this(LocateDataException.Default, ContainsRetryCode.Default.Then().Or(NetworkRelatedException.Default)) {}

	readonly ISelect<Exception, SqlException?> _locate;
	readonly Func<SqlException, bool>          _predicate;

	public IsDataException(ISelect<Exception, SqlException?> locate, Func<SqlException, bool> predicate)
	{
		_locate    = locate;
		_predicate = predicate;
	}

	public bool Get(Exception parameter) => _locate.Get(parameter) is {} data && _predicate(data);
}