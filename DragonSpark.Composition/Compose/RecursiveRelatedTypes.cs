using System;
using System.Collections.Generic;
using DragonSpark.Model.Sequences.Memory;
using JetBrains.Annotations;

namespace DragonSpark.Composition.Compose;

sealed class RecursiveRelatedTypes : IRelatedTypes
{
    readonly IRelatedTypes _related;
    readonly INewLeasing<Type> _new;

    public RecursiveRelatedTypes(IRelatedTypes related) : this(related, NewLeasing<Type>.Default) { }

    public RecursiveRelatedTypes(IRelatedTypes related, INewLeasing<Type> @new)
    {
        _related = related;
        _new = @new;
    }

    void Expand(HashSet<Type> state, Type current)
    {
        using var related = _related.Get(current);
        var length = related.Length;
        var destination = related.AsSpan();
        for (var i = 0; i < length; i++)
        {
            var item = destination[i];
            if (state.Add(item))
            {
                Expand(state, item);
            }
        }
    }

    [MustDisposeResource]
    public Leasing<Type> Get(Type parameter)
    {
        var state = new HashSet<Type>();
        Expand(state, parameter);
        var count = (uint)state.Count;
        var lease = _new.Get(count);
        var index = 0;
        var destination = lease.AsSpan();
        foreach (var type in state)
        {
            destination[index++] = type;
        }

        var result = lease.Size(index);
        return result;
    }
}
