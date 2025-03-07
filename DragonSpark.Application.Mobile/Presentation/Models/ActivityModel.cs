using System;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using Uno.Extensions.Reactive;
using ICommand = System.Windows.Input.ICommand;

namespace DragonSpark.Application.Mobile.Presentation.Models;

public class ActivityModel<TIn, TOut> : ISelecting<Token<TIn>, TOut>, ICommand
{
    readonly ICondition                         _update;
    readonly IMutable<CancellationTokenSource?> _active;
    readonly ISelecting<Token<TIn>, TOut>       _previous;

    public ActivityModel(ISelecting<Token<TIn>, TOut> previous, IDispatches dispatches)
        : this(new Variable<CancellationTokenSource?>(), previous, dispatches) {}

    public ActivityModel(IMutable<CancellationTokenSource?> active, ISelecting<Token<TIn>, TOut> previous,
                         IDispatches dispatches)
    {
        _active   = active;
        _previous = previous;
        Active    = _active.AsStateAssigned(this);
        _update   = dispatches.Get(Update);
    }

    public IState<bool> Active { get; }
    public event EventHandler? CanExecuteChanged = delegate {};

    void Update()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool CanExecute(object? parameter)
    {
        return _active.Get() is not null;
    }

    public void Execute(object? parameter)
    {
        if (_active.TryPop(out var source))
        {
            source.Verify().Cancel();
            _update.Get();
        }
    }

    public async ValueTask<TOut> Get(Token<TIn> parameter)
    {
        try
        {
            var (_, item) = parameter;
            var source = CancellationTokenSource.CreateLinkedTokenSource(item);
            _active.Execute(source);
            _update.Get();
            return await _previous.Await(parameter with { Item = source.Token });
        }
        finally
        {
            _active.Execute(null);
        }
    }
}