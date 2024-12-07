﻿using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations.Execution;

sealed class ProcessOperations : IOperation
{
	readonly DeferredOperationsQueue _queue;

	public ProcessOperations(DeferredOperationsQueue queue) => _queue = queue;

	public async ValueTask Get()
	{
		while (_queue.TryDequeue(out var operation))
		{
			await operation().Await();
		}

		_queue.Clear();
	}
}