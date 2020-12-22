using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Types;
using Microsoft.Extensions.DependencyInjection;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;

namespace DragonSpark.Composition.Compose
{
	sealed class DependencyRelatedTypes : IRelatedTypes
	{
		readonly Predicate<Type>    _can;
		readonly IArray<Type, Type> _candidates;

		public DependencyRelatedTypes(IServiceCollection services)
			: this(new CanRegister(services).Then()
			                                .And(IsNativeSystemType.Default.Then().Inverse())
			                                .And(new HashSet<Type>().Add),
			       DependencyCandidates.Default) {}

		public DependencyRelatedTypes(Predicate<Type> can, IArray<Type, Type> candidates)
		{
			_can        = can;
			_candidates = candidates;
		}

		public Array<Type> Get(Type parameter)
			=> _candidates.Get(parameter).Open().AsValueEnumerable().Where(_can).ToArray();
	}
}