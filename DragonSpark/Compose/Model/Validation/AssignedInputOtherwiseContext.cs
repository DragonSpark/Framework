using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model.Validation;

public sealed class AssignedInputOtherwiseContext<TIn, TOut> : InputOtherwiseContext<TIn, TOut>
{
	public AssignedInputOtherwiseContext(ISelect<TIn, TOut> subject) : base(subject, Is.Assigned<TIn>()) {}

	public ConditionalSelector<TIn, TOut> Throw() => Throw(AssignedArgumentMessage.Default);

	public ConditionalSelector<TIn, TOut> Throw(IResult<string> message)
		=> Throw(message.Then().Accept<Type>().Return());

	public ConditionalSelector<TIn, TOut> Throw(ISelect<Type, string> message)
		=> Throw(message.Then().Out<TIn>().Get());

	public ConditionalSelector<TIn, TOut> Throw(ISelect<TIn, string> message)
		=> new AssignedInputThrowContext<TIn, TOut>(this).WithMessage(message);
}