using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Diagnostics;

sealed class LastChanceExceptionHandler : ConditionAware<Exception>, ILastChanceExceptionHandler
{
    readonly IApplicationErrorHandler _handler;

    public LastChanceExceptionHandler(IApplicationErrorHandler handler) : this(handler, Is.Always<Exception>().Out()) {}

    public LastChanceExceptionHandler(IApplicationErrorHandler handler, ICondition<Exception> condition)
        : base(condition)
        => _handler = handler;

    public ValueTask Get(Token<Exception> parameter)
    {
        _handler.Execute(parameter);
        return ValueTask.CompletedTask;
    }
}