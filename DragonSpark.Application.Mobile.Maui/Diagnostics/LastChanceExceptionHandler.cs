using DragonSpark.Application.Mobile.Diagnostics;
using DragonSpark.Application.Mobile.Maui.Presentation.Components.Notification;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

sealed class LastChanceExceptionHandler : ConditionAware<Exception>, ILastChanceExceptionHandler
{
    readonly ILastChanceExceptionHandler _previous;
    readonly IStopAware<ToastInput>      _display;

    public LastChanceExceptionHandler(ILastChanceExceptionHandler previous)
        : this(previous, MainThreadAwareDisplayToast.Default) {}

    public LastChanceExceptionHandler(ILastChanceExceptionHandler previous, IStopAware<ToastInput> display)
        : base(previous.Condition)
    {
        _previous = previous;
        _display  = display;
    }

    public async ValueTask Get(Stop<Exception> parameter)
    {
        var (_, stop) = parameter;
        await _previous.On(parameter);
        await _display.Off(new(new("A problem was encountered and has been logged for administrative review"), stop));
    }
}