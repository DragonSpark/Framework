using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Compose.Model.Operations;
using DragonSpark.Model.Operations;
using System;

namespace DragonSpark.Application.Compose
{
	// TODO: Rename
	public class OperationResultSelectorExtended<_, T> : OperationResultSelector<_, T>
	{
		readonly ISelecting<_, T> _subject;

		public OperationResultSelectorExtended(ISelecting<_, T> subject) : base(subject) => _subject = subject;

		public OperationResultSelectorExtended<_, T> Handle<TReported>(IExceptions exceptions)
			=> Handle(exceptions, A.Type<TReported>());

		public OperationResultSelectorExtended<_, T> Handle(IExceptions exceptions, Type reportedType)
			=> new(new ExceptionAwareSelecting<_, T>(_subject, exceptions, reportedType));
	}
}