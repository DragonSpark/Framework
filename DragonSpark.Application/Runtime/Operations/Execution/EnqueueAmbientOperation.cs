using DragonSpark.Compose;
using DragonSpark.Compose.Model.Operations;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations.Execution;

public class EnqueueAmbientOperation : Command
{
	protected EnqueueAmbientOperation(IAmbientOperations operations, IOperation subject)
		: this(operations, subject.Get) {}

	protected EnqueueAmbientOperation(IAmbientOperations operations, Func<ValueTask> operation)
		: base(operations.Then().Bind(operation)) {}
}


public class EnqueueAmbientOperation<T> : ICommand<T>
{
	readonly IAmbientOperations  _operations;
	readonly OperationContext<T> _subject;

	protected EnqueueAmbientOperation(IAmbientOperations operations, IOperation<T> subject)
		: this(operations, subject.Then()) {}

	protected EnqueueAmbientOperation(IAmbientOperations operations, OperationContext<T> subject)
	{
		_operations = operations;
		_subject    = subject;
	}

	public void Execute(T parameter)
	{
		_operations.Execute(_subject.Bind(parameter));
	}
}