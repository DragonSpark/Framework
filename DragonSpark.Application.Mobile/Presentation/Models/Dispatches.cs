using System;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.Extensions.DependencyInjection;
using Uno.Extensions;

namespace DragonSpark.Application.Mobile.Presentation.Models;

sealed class Dispatches : IDispatches
{
    readonly IDispatcher _dispatcher;

    public Dispatches(IDispatcher dispatcher) => _dispatcher = dispatcher;

    public ICondition Get(Action parameter) => new Dispatch(_dispatcher, parameter);
}

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<IDispatches>().Forward<Dispatches>().Scoped();
    }
}