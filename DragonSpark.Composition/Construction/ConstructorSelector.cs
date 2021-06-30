using DragonSpark.Model.Sequences.Memory;
using LightInject;
using NetFabric.Hyperlinq;
using System;
using System.Reflection;

namespace DragonSpark.Composition.Construction
{
	sealed class ConstructorSelector : IConstructorSelector
	{
		readonly Predicate<ParameterInfo>           _specification;
		readonly ILease<Type, ConstructorCandidate> _candidates;

		public ConstructorSelector(Predicate<ParameterInfo> specification)
			: this(specification, ConstructorCandidates.Default) {}

		public ConstructorSelector(Predicate<ParameterInfo> specification,
		                           ILease<Type, ConstructorCandidate> candidates)
		{
			_specification = specification;
			_candidates    = candidates;
		}

		public ConstructorInfo Execute(Type implementingType)
		{
			using var candidates = _candidates.Get(implementingType);

			var length = candidates.Length;
			if (length == 0)
			{
				throw new
					InvalidOperationException($"Missing public constructor for Type: {implementingType.FullName}");
			}

			var span = candidates.AsSpan();
			for (var index = 0; index < length; index++)
			{
				var (candidate, parameters) = span[index];
				if (parameters.AsValueEnumerable().All(_specification))
				{
					return candidate;
				}
			}

			throw new
				InvalidOperationException($"No resolvable constructor found for Type: {implementingType.FullName}");
		}
	}
}