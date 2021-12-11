using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

sealed class WorkerOperation : IAllocated
{
	readonly Task                 _subject;
	readonly TaskCompletionSource _source;
	readonly Action               _complete;

	public WorkerOperation(Task subject, TaskCompletionSource source, Action complete)
	{
		_subject  = subject;
		_source   = source;
		_complete = complete;
	}

	public async Task Get()
	{
		try
		{
			await _subject;
			_source.SetResult();
		}
		// ReSharper disable once CatchAllClause
		catch (Exception e)
		{
			_source.SetException(e);
		}
		finally
		{
			_complete();
		}
	}
}

sealed class WorkerOperation<T> : IOperation
{
	readonly Task<T>                 _subject;
	readonly TaskCompletionSource<T> _source;
	readonly Action                  _complete;

	public WorkerOperation(Task<T> subject, TaskCompletionSource<T> source, Action complete)
	{
		_subject  = subject;
		_source   = source;
		_complete = complete;
	}

	public async ValueTask Get()
	{
		try
		{
			var content = await _subject;
			_source.SetResult(content);
		}
		// ReSharper disable once CatchAllClause
		catch (Exception e)
		{
			_source.SetException(e);
		}
		finally
		{
			_complete();
		}
	}
}