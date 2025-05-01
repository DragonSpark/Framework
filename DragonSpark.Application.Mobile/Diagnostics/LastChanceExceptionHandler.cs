using System;

namespace DragonSpark.Application.Mobile.Diagnostics;

sealed class LastChanceExceptionHandler : ILastChanceExceptionHandler
{
    readonly IApplicationErrorHandler _handler;

    public LastChanceExceptionHandler(IApplicationErrorHandler handler) => _handler = handler;

    public bool Get(Exception parameter) => true;

    public void Execute(Exception parameter)
    {
        _handler.Execute(parameter);
    }
}