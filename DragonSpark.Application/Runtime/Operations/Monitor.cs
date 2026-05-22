using DragonSpark.Compose;
using DragonSpark.Model.Operations.Allocated;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

readonly struct Monitor : IAllocated
{
	readonly Task         _subject;
	readonly Action<Task> _complete;

	public Monitor(Task subject, Action<Task> complete)
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
			_complete(_subject);
		}
	}
}

readonly struct Monitor<T> : IAllocated
{
	readonly Task<T?>   _subject;
	readonly IAllocated _complete;

	public Monitor(Task<T?> subject, IAllocated complete)
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