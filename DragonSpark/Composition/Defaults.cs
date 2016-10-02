using DragonSpark.Activation;
using DragonSpark.Specifications;
using System;

namespace DragonSpark.Composition
{
	public static class Defaults
	{
		public static ISpecification<Type> ConventionCandidate { get; } = CanActivateSpecification.Default.Inverse();
	}
}