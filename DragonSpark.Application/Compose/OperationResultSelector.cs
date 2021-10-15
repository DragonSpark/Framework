using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
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

	public OperationResultSelector<_, T> Handle<TReported>(IExceptions exceptions, T @default)
		=> Handle(exceptions, A.Type<TReported>(), @default);

	public OperationResultSelector<_, T> Handle(IExceptions exceptions, Type reportedType, T @default)
		=> new(new ExceptionAwareSelectingDefault<_, T>(new ExceptionAwareSelecting<_, T>(_subject, exceptions, reportedType), @default));
}