using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model.Validation;

public sealed class AssignedOutputOtherwiseComposer<TIn, TOut> : OutputOtherwiseComposer<TIn, TOut>
{
	public AssignedOutputOtherwiseComposer(ISelect<TIn, TOut> subject)
		: base(subject, Is.Assigned<TOut>()) {}

	public Composer<TIn, TOut> Throw() => Throw(AssignedResultMessage.Default);

	public Composer<TIn, TOut> Throw(IResult<string> message)
		=> Throw(message.Then().Accept<Type>().Return());

	public Composer<TIn, TOut> Throw(ISelect<Type, string> message)
		=> Throw(Start.A.Selection<TIn>()
		              .AndOf<Type>()
		              .By.Cast.Or.Return(A.Type<TOut>())
		              .Select(message));

	public Composer<TIn, TOut> Throw(ISelect<TIn, string> message) => Throw(message.Get);

	public Composer<TIn, TOut> Throw(Func<TIn, string> message)
		=> new AssignedOutputThrowComposer<TIn, TOut>(this).WithMessage(message);
}