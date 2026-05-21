using DragonSpark.Compose;
using DragonSpark.Model.Operations.Allocated;
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
			await _subject.On();
			_source.SetResult();
		}
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

readonly struct WorkerMonitor<T> : IAllocated
{
	readonly Task<T?> _subject;
	readonly IAllocated    _complete;

	public WorkerMonitor(Task<T?> subject, IAllocated complete)
	{
		_subject  = subject;
		_complete = complete;
	}

	public async Task Get()
	{
		try
		{
			await _subject.On();
		}
		catch
		{
			// ignored: handled in _complete below
		}
		finally
		{
			await _complete.Off();
		}
	}
}