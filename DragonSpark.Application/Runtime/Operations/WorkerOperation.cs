using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

sealed class WorkerOperation : IAllocated
{
	readonly Task                 _subject;
	readonly TaskCompletionSource _source;

	public WorkerOperation(Task subject, TaskCompletionSource source)
	{
		_subject = subject;
		_source  = source;
	}

	public async Task Get()
	{
		try
		{
			await _subject.ConfigureAwait(false);
			_source.SetResult();
		}
		// ReSharper disable once CatchAllClause
		catch (Exception e)
		{
			_source.SetException(e);
		}
	}
}

sealed class WorkerOperation<T> : IAllocated
{
	readonly Task<T>                 _subject;
	readonly TaskCompletionSource<T> _source;

	public WorkerOperation(Task<T> subject, TaskCompletionSource<T> source)
	{
		_subject = subject;
		_source  = source;
	}

	public async Task Get()
	{
		try
		{
			var content = await _subject.ConfigureAwait(false);
			_source.SetResult(content);
		}
		// ReSharper disable once CatchAllClause
		catch (Exception e)
		{
			_source.SetException(e);
		}
	}
}