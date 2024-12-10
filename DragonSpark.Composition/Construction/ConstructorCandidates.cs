using System;
using System.Buffers;
using System.Linq;
using System.Reflection;
using DragonSpark.Model.Sequences.Memory;
using JetBrains.Annotations;
using NetFabric.Hyperlinq;

namespace DragonSpark.Composition.Construction;

sealed class ConstructorCandidates : ILease<Type, ConstructorCandidate>
{
    public static ConstructorCandidates Default { get; } = new();

    ConstructorCandidates() : this(ArrayPool<ConstructorCandidate>.Shared, IsCandidate.Default.Get) { }

    readonly ArrayPool<ConstructorCandidate> _pool;
    readonly Func<ConstructorCandidate, bool> _candidate;

    public ConstructorCandidates(ArrayPool<ConstructorCandidate> pool, Func<ConstructorCandidate, bool> candidate)
    {
        _pool = pool;
        _candidate = candidate;
    }

    [MustDisposeResource]
    public Leasing<ConstructorCandidate> Get(Type parameter)
        => new(parameter.GetTypeInfo()
                        .DeclaredConstructors.Select(x => new ConstructorCandidate(x))
                        .OrderByDescending(x => x.Parameters.Length)
                        .AsValueEnumerable()
                        .Where(_candidate)
                        .ToArray(_pool));
}
