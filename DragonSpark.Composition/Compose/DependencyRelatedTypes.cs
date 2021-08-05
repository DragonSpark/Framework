using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Memory;
using DragonSpark.Reflection.Types;
using Microsoft.Extensions.DependencyInjection;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;

namespace DragonSpark.Composition.Compose
{
	sealed class DependencyRelatedTypes : IRelatedTypes
	{
		readonly Func<Type, bool>   _can;
		readonly IArray<Type, Type> _candidates;
		readonly ILeases<Type>      _leases;

		public DependencyRelatedTypes(IServiceCollection services)
			: this(new CanRegister(services).Then()
			                                .And(IsNativeSystemType.Default.Then().Inverse())
			                                .And(new HashSet<Type>().Add),
			       DependencyCandidates.Default, Leases<Type>.Default) {}

		public DependencyRelatedTypes(Func<Type, bool> can, IArray<Type, Type> candidates, ILeases<Type> leases)
		{
			_can        = can;
			_candidates = candidates;
			_leases     = leases;
		}

		public Lease<Type> Get(Type parameter)
		{
			var types       = _candidates.Get(parameter).Open();
			var result      = _leases.Get(types.Length);
			var index       = 0;
			var destination = result.AsSpan();
			foreach (var type in types.AsValueEnumerable().Where(_can))
			{
				destination[index++] = type;
			}

			return result.Size(index);
		}
	}
}