using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations.Execution;

sealed class Operations : IOperations
{
	readonly ProcessOperations       _process;
	readonly DeferredOperationsQueue _subject;

	public Operations(ProcessOperations process, DeferredOperationsQueue subject)
	{
		_process = process;
		_subject = subject;
	}

	public ValueTask Get() => _process.Get();

	public void Execute(Func<ValueTask> parameter)
	{
		_subject.Enqueue(parameter);
	}
}