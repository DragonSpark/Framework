using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

sealed class ExceptionAwareAny<T> : IAny<T>
{
	readonly IAny<T>     _previous;
	readonly IExceptions _exceptions;

	public ExceptionAwareAny(IAny<T> previous, IExceptions exceptions)
	{
		_previous   = previous;
		_exceptions = exceptions;
	}

	public ValueTask<bool> Get(AnyInput<T> parameter)
	{
		var (owner, _) = parameter;
		var type = owner.AsTo<IReportedTypeAware, Type?>(x => x.Get()) ?? owner.GetType();
		return new Selection(_previous, _exceptions, type).Get(parameter);
	}

	sealed class Selection : ExceptionAwareSelecting<AnyInput<T>, bool>
	{
		public Selection(IAny<T> previous, IExceptions exceptions, Type? reportedType = null)
			: base(previous, exceptions, reportedType) {}
	}
}