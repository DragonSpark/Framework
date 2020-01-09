using DragonSpark.Model;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model.Validation {
	public sealed class AssignedOutputOtherwiseContext<TIn, TOut> : OutputOtherwiseContext<TIn, TOut>
	{
		public AssignedOutputOtherwiseContext(ISelect<TIn, TOut> subject)
			: base(subject, Is.Assigned<TOut>().Get) {}

		public Selector<TIn, TOut> Throw() => Throw(AssignedResultMessage.Default);

		public Selector<TIn, TOut> Throw(IResult<string> message)
			=> Throw(message.Then().Accept<Type>().Return());

		public Selector<TIn, TOut> Throw(ISelect<Type, string> message)
			=> Throw(Start.A.Selection<TIn>()
			              .AndOf<Type>()
			              .By.Cast.Or.Return(A.Type<TOut>())
			              .Then()
			              .Select(message)
			              .Get());

		public Selector<TIn, TOut> Throw(ISelect<TIn, string> message)
			=> new AssignedOutputThrowContext<TIn, TOut>(this).WithMessage(message);
	}
}