using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Compose.Model.Selection;

public sealed class ConditionalComposer<TIn, TOut> : Composer<TIn, TOut>, IResult<IConditional<TIn, TOut>>
{
	public static implicit operator Func<TIn, TOut>(ConditionalComposer<TIn, TOut> instance) => instance.Get().Get;

	readonly IConditional<TIn, TOut> _subject;

	public ConditionalComposer(IConditional<TIn, TOut> subject) : base(subject) => _subject = subject;

	public new IConditional<TIn, TOut> Get() => _subject;
}