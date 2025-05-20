using System;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Diagnostics;
using DragonSpark.Application.Mobile.Maui.Presentation.Components.Notification;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

sealed class LastChanceExceptionHandler : ConditionAware<Exception>, ILastChanceExceptionHandler
{
    readonly ILastChanceExceptionHandler _previous;
    readonly DisplayToast                _display;

    public LastChanceExceptionHandler(ILastChanceExceptionHandler previous) : this(previous, DisplayToast.Default) {}

    public LastChanceExceptionHandler(ILastChanceExceptionHandler previous, DisplayToast display)
        : base(previous.Condition)
    {
        _previous = previous;
        _display  = display;
    }

    public async ValueTask Get(Stop<Exception> parameter)
    {
        await _previous.On(parameter);
        await _display.Off(new("A problem was encountered and has been logged for administrative review"));
    }
}