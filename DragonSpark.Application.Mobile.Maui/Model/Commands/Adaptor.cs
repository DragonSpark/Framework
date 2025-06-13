using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;

namespace DragonSpark.Application.Mobile.Maui.Model.Commands;

sealed class Adaptor<T> : AsynchronousCommand<T>
{
    public Adaptor(IOperation<T> input) : this(input!.Allocate) {}

    public Adaptor(IAllocated<T> input) : this(input.Then()!) {}
    public Adaptor(Func<T?, Task> execute) : base(execute) {}
}