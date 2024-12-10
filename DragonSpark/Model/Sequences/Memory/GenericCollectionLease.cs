using System.Collections.Generic;
using DragonSpark.Runtime;
using JetBrains.Annotations;

namespace DragonSpark.Model.Sequences.Memory;

sealed class GenericCollectionLease<T> : ILease<ICollection<T>, T>
{
    public static GenericCollectionLease<T> Default { get; } = new();

    GenericCollectionLease() { }

    [MustDisposeResource]
    public Leasing<T> Get(ICollection<T> parameter)
    {
        using var builder = ArrayBuilder.New<T>(parameter.Count);
        builder.Add(parameter);
        var result = builder.AsLease();
        return result;
    }
}
