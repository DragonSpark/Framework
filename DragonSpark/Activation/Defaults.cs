using DragonSpark.Activation.Location;
using DragonSpark.Specifications;
using System;

namespace DragonSpark.Activation
{
	public static class Defaults
	{
		public static ISpecification<Type> Instantiable { get; } = CanActivateSpecification.Default.Or( ContainsSingletonPropertySpecification.Default ).ToCachedSpecification();
	}
}
