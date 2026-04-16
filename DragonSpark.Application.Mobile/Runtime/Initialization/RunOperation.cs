using System;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

sealed class RunOperation : IOperation<IStopAware>
{
    public static RunOperation Default { get; } = new();

    RunOperation() : this(HandleInitializationException.Default) {}

    readonly ICommand<Exception> _exception;

    public RunOperation(ICommand<Exception> exception) => _exception = exception;

    public async ValueTask Get(IStopAware parameter)
    {
        try
        {
            await parameter.Off(CancellationToken.None);
        }
        catch (Exception e)
        {
            _exception.Execute(e);
            throw;
        }
    }
}