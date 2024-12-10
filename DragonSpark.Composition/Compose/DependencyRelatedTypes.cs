using System;
using System.Collections.Generic;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Memory;
using DragonSpark.Reflection.Types;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using NetFabric.Hyperlinq;

namespace DragonSpark.Composition.Compose;

sealed class DependencyRelatedTypes : IRelatedTypes
{
    readonly Func<Type, bool> _can;
    readonly IArray<Type, Type> _candidates;
    readonly INewLeasing<Type> _new;

    public DependencyRelatedTypes(IServiceCollection services)
        : this(new CanRegister(services).Then()
                                        .And(IsNativeSystemType.Default.Then().Inverse())
                                        .And(new HashSet<Type>().Add),
               DependencyCandidates.Default, NewLeasing<Type>.Default)
    { }

    public DependencyRelatedTypes(Func<Type, bool> can, IArray<Type, Type> candidates, INewLeasing<Type> @new)
    {
        _can = can;
        _candidates = candidates;
        _new = @new;
    }

    [MustDisposeResource]
    public Leasing<Type> Get(Type parameter)
    {
        var types = _candidates.Get(parameter).Open();
        var result = _new.Get(types.Length);
        var index = 0;
        var destination = result.AsSpan();
        foreach (var type in types.AsValueEnumerable().Where(_can))
        {
            destination[index++] = type;
        }

        return result.Size(index);
    }
}
