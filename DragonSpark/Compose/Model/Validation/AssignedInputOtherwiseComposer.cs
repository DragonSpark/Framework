using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model.Validation;

public sealed class AssignedInputOtherwiseComposer<TIn, TOut> : InputOtherwiseComposer<TIn, TOut>
{
	public AssignedInputOtherwiseComposer(ISelect<TIn, TOut> subject) : base(subject, Is.Assigned<TIn>()) {}

	public ConditionalComposer<TIn, TOut> Throw() => Throw(AssignedArgumentMessage.Default);

	public ConditionalComposer<TIn, TOut> Throw(IResult<string> message)
		=> Throw(message.Then().Accept<Type>().Return());

	public ConditionalComposer<TIn, TOut> Throw(ISelect<Type, string> message)
		=> Throw(message.Then().Out<TIn>().Get());

	public ConditionalComposer<TIn, TOut> Throw(ISelect<TIn, string> message)
		=> new AssignedInputThrowComposer<TIn, TOut>(this).WithMessage(message);
}