using DragonSpark.Application.Diagnostics;
using System;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

sealed class ExceptionAwarePaging<T> : IPaging<T>
{
	readonly IPaging<T>  _previous;
	readonly IExceptions _exceptions;

	public ExceptionAwarePaging(IPaging<T> previous, IExceptions exceptions)
	{
		_previous   = previous;
		_exceptions = exceptions;
	}

	public IPages<T> Get(PagingInput<T> parameter)
	{
		var (owner, _, _) = parameter;
		return new Report(_previous.Get(parameter), _exceptions, owner.Get());
	}

	sealed class Report : ExceptionAwareSelecting<PageInput, Page<T>>, IPages<T>
	{
		public Report(IPages<T> previous, IExceptions exceptions, Type? reportedType = null)
			: base(previous, exceptions, reportedType) {}
	}
}