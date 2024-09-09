using DragonSpark.Application.Diagnostics;
using DragonSpark.Application.Runtime;
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

	public OperationComposer<T> Catch<TReported>(IExceptions exceptions)
		=> Catch(exceptions, A.Type<TReported>());

	public OperationComposer<T> Catch(IExceptions exceptions, Type reportedType)
		=> new(new ExceptionAware<T>(_subject, exceptions, reportedType));

	public OperationComposer<T> Handle<TReported>(IExceptions exceptions)
		=> Handle(exceptions, A.Type<TReported>());

	public OperationComposer<T> Handle(IExceptions exceptions, Type reportedType)
		=> new(new ThrowingAware<T>(_subject, exceptions, reportedType));

#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
	public OperationComposer<T> Throttle(TimeSpan @for)
		=> new(new ThrottleOperation<T>(_subject.Get, @for));
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
}