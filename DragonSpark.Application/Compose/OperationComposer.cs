using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Compose.Model.Operations;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Compose;

public class OperationComposer<T> : OperationContext<T>
{
	readonly ISelect<T, ValueTask> _subject;

	public OperationComposer(ISelect<T, ValueTask> subject) : base(subject) => _subject = subject;

	public OperationComposer<T> Handle<TReported>(IExceptionLogger exceptions)
		=> Handle(exceptions, A.Type<TReported>());

	public OperationComposer<T> Handle(IExceptionLogger exceptions, Type reportedType)
		=> new(new ExceptionLoggingAware<T>(_subject, exceptions, reportedType));

	public OperationComposer<T> Catch<TReported>(IExceptions exceptions)
		=> Catch(exceptions, A.Type<TReported>());

	public OperationComposer<T> Catch(IExceptions exceptions, Type reportedType)
		=> new(new ExceptionAware<T>(_subject, exceptions, reportedType));

	public OperationComposer<T> Handle<TReported>(IExceptions exceptions)
		=> Handle(exceptions, A.Type<TReported>());

	public OperationComposer<T> Handle(IExceptions exceptions, Type reportedType)
		=> new(new ThrowingAware<T>(_subject, exceptions, reportedType));
}