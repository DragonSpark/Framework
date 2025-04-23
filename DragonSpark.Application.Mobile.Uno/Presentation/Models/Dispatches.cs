using System;
using DragonSpark.Model.Selection.Conditions;
using Uno.Extensions;

namespace DragonSpark.Application.Mobile.Uno.Presentation.Models;

sealed class Dispatches : IDispatches
{
    readonly IDispatcher _dispatcher;

    public Dispatches(IDispatcher dispatcher) => _dispatcher = dispatcher;

    public ICondition Get(Action parameter) => new Dispatch(_dispatcher, parameter);
}