using System;
using DragonSpark.Model.Results;

namespace DragonSpark.Model.Selection.Conditions;

public class Assume<TIn, TOut> : IConditional<TIn, TOut>
{
    readonly Func<IConditional<TIn, TOut>> _previous;

    public Assume(IResult<IConditional<TIn, TOut>> source) : this(source.Get) {}

    public Assume(Func<IConditional<TIn, TOut>> previous) => _previous = previous;

    public ICondition<TIn> Condition => _previous().Condition;

    public TOut Get(TIn parameter) => _previous().Get(parameter);
}