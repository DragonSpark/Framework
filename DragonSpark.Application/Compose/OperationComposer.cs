using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Compose;

public class OperationComposer<T> : DragonSpark.Compose.Model.Operations.OperationComposer<T>
{
	readonly ISelect<T, ValueTask> _subject;

	public OperationComposer(ISelect<T, ValueTask> subject) : base(subject) => _subject = subject;

	public OperationComposer<T> Handle<TReported>(IExceptionLogger exceptions)
		=> Handle(exceptions, A.Type<TReported>());

	public OperationComposer<T> Handle(IExceptionLogger exceptions, Type reportedType)
		=> new(new ExceptionLoggingAware<T>(_subject, exceptions, reportedType));

	public OperationComposer<T> Throw<TReported>(IExceptions exceptions)
		=> Throw(exceptions, A.Type<TReported>());

	public OperationComposer<T> Throw(IExceptions exceptions, Type reportedType)
		=> new(new ThrowingAware<T>(_subject, exceptions, reportedType));
}