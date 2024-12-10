using System;
using DragonSpark.Model.Sequences.Memory;
using JetBrains.Annotations;

namespace DragonSpark.Application.Model.Sequences;

[method: MustDisposeResource]
public readonly record struct Transactions<T>(Leasing<T> Add, Leasing<Update<T>> Update, Leasing<T> Delete)
    : IDisposable
{
    public bool Any() => Add.Length > 0 || Update.Length > 0 || Delete.Length > 0;

    public TransactionSpans<T> AsSpans() => new(Add.AsMemory(), Update.AsMemory(), Delete.AsMemory());
    public TransactionMemory<T> AsMemory() => new(Add.AsMemory(), Update.AsMemory(), Delete.AsMemory());

    public void Dispose()
    {
        Add.Dispose();
        Update.Dispose();
        Delete.Dispose();
    }
}
