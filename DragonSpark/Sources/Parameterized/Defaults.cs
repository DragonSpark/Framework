using DragonSpark.Sources.Delegates;
using DragonSpark.Specifications;
using System;

namespace DragonSpark.Sources.Parameterized
{
	public static class Defaults
	{
		public static object Parameter { get; } = new object();

		public static ISpecification<Type> KnownSourcesSpecification { get; } = IsSourceSpecification.Default.Or( IsParameterizedSourceSpecification.Default ).ToCachedSpecification();
	}
}