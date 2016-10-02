using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized.Caching;
using System;

namespace DragonSpark.Composition
{
	sealed class ConventionCandidateNames : Cache<Type, string>
	{
		public static ConventionCandidateNames Default { get; } = new ConventionCandidateNames();
		ConventionCandidateNames() : base( type => type.Name.TrimStartOf( 'I' ) ) {}
	}
}