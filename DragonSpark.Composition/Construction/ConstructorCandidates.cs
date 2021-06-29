using DragonSpark.Compose;
using DragonSpark.Model.Sequences.Memory;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Composition.Construction
{
	sealed class ConstructorCandidates : ILease<Type, ConstructorCandidate>
	{
		public static ConstructorCandidates Default { get; } = new ConstructorCandidates();

		ConstructorCandidates() : this(MemoryPool<ConstructorCandidate>.Shared) {}

		readonly MemoryPool<ConstructorCandidate> _pool;

		public ConstructorCandidates(MemoryPool<ConstructorCandidate> pool) => _pool = pool;

		public Lease<ConstructorCandidate> Get(Type parameter)
			=> new(parameter.GetTypeInfo()
			                .DeclaredConstructors.Select(x => new ConstructorCandidate(x))
			                .OrderByDescending(x => x.Parameters.Length)
			                .AsValueEnumerable()
			                .Where(c => c.Constructor.IsPublic &&
			                            !c.Constructor.IsStatic)
			                .Where(x => x.Constructor.Attribute<CandidateAttribute>()
			                             ?.Enabled ?? true)
			                .ToArray(_pool));
	}
}