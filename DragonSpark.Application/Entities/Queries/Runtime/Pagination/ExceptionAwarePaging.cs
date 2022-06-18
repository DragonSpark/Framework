using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
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
		var type = owner.AsTo<IReportedTypeAware, Type?>(x => x.Get()) ?? owner.GetType();
		return new Pages(_previous.Get(parameter), _exceptions, type);
	}

	sealed class Pages : ExceptionAwareSelecting<PageInput, Page<T>>, IPages<T>
	{
		public Pages(IPages<T> previous, IExceptions exceptions, Type? reportedType = null)
			: base(previous, exceptions, reportedType) {}
	}
}