using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;

namespace DragonSpark.Model.Operations.Selection;

public class Assuming<TIn, TOut> : ISelecting<TIn, TOut>
{
    readonly IResulting<ISelecting<TIn, TOut>> _previous;

    protected Assuming(Func<ISelecting<TIn, TOut>> previous) : this(previous.Start().Operation().Out()) {}

    protected Assuming(IResulting<ISelecting<TIn, TOut>> previous) => _previous = previous;

    public async ValueTask<TOut> Get(TIn parameter)
    {
        var previous = await _previous.Off();
        return await previous.Off(parameter);
    }
}