using System;
using System.Threading.Tasks;
using DragonSpark.Compose;

namespace DragonSpark.Application.Diagnostics;

sealed class ExecuteOperation : IExecuteOperation
{
    readonly IExceptions _exceptions;

    public ExecuteOperation(IExceptions exceptions) => _exceptions = exceptions;

    public async ValueTask<Exception?> Get((Type Owner, ValueTask Operation) parameter)
    {
        var (owner, operation) = parameter;
        try
        {
            if (!operation.IsCompleted)
            {
                await operation.ConfigureAwait(false);
            }
            else if (operation.IsFaulted)
            {
                // ReSharper disable once UnthrowableException
                throw operation.AsTask().Exception.Verify();
            }

            return null;
        }
        // ReSharper disable once CatchAllClause
        catch (Exception error)
        {
            await _exceptions.Await(new(owner, error));
            return error;
        }
    }
}
