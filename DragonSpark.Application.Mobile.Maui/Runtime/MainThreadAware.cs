using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.Maui.ApplicationModel;

namespace DragonSpark.Application.Mobile.Maui.Runtime;

public class MainThreadAware<TIn, TOut> : IStopAware<TIn, TOut>
{
    readonly IStopAware<TIn, TOut> _previous;

    protected MainThreadAware(IStopAware<TIn, TOut> previous) => _previous = previous;

    public ValueTask<TOut> Get(Stop<TIn> parameter)
        => MainThread.InvokeOnMainThreadAsync(() => _previous.Allocate(parameter)).ToOperation();
}