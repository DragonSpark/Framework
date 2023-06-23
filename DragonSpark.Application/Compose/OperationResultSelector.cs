using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using System;

namespace DragonSpark.Application.Compose;

public class OperationResultSelector<_, T> : DragonSpark.Compose.Model.Operations.OperationResultSelector<_, T>
{
	readonly ISelecting<_, T> _subject;

	public OperationResultSelector(ISelecting<_, T> subject) : base(subject) => _subject = subject;

	public OperationResultSelector<_, T> Handle<TReported>(IExceptions exceptions)
		=> Handle(exceptions, A.Type<TReported>());

	public OperationResultSelector<_, T> Handle(IExceptions exceptions, Type reportedType)
		=> new(new ExceptionAwareSelecting<_, T>(_subject, exceptions, reportedType));

	public OperationResultSelector<_, T> Handle(T @default)
		=> new(new ExceptionAwareSelectingDefault<_, T>(_subject, @default));
}