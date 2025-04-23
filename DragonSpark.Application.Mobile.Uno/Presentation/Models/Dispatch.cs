using System;
using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;
using Uno.Extensions;

namespace DragonSpark.Application.Mobile.Uno.Presentation.Models;

sealed class Dispatch : ICondition
{
    readonly IDispatcher _dispatcher;
    readonly Action      _action;

    public Dispatch(IDispatcher dispatcher, Action action)
    {
        _dispatcher = dispatcher;
        _action     = action;
    }

    public bool Get(None parameter) => _dispatcher.TryEnqueue(_action);
}