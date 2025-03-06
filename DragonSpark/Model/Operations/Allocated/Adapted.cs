using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated;

public class Adapted : IAllocated
{
    readonly Action _action;

    protected Adapted(Action action) => _action = action;

    public Task Get()
    {
        _action();
        return Task.CompletedTask;
    }
}