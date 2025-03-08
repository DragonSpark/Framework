using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Diagnostics;

public class ThrowingAware<T> : IOperation<T>
{
	readonly ISelect<T, ValueTask> _previous;
	readonly IExceptions           _exceptions;
	readonly Type?                 _reportedType;

	public ThrowingAware(ISelect<T, ValueTask> previous, IExceptions exceptions, Type? reportedType = null)
	{
		_previous     = previous;
		_exceptions   = exceptions;
		_reportedType = reportedType;
	}

	public async ValueTask Get(T parameter)
	{
		try
		{
			await _previous.Off(parameter);
		}
		catch (Exception e)
		{
			await _exceptions.Off(new(_reportedType ?? GetType(), e));
			throw;
		}
	}
}