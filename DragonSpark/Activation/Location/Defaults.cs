using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Specifications;
using System;
using System.Reflection;

namespace DragonSpark.Activation.Location
{
	public static class Defaults
	{
		public static ISpecification<SingletonRequest> SourcedSingleton { get; } = SingletonSpecification.Default.Project<SingletonRequest, PropertyInfo>( request => request.Candidate ).And( SourceTypeAssignableSpecification.Default.Project<SingletonRequest, SourceTypeCandidateParameter>( destination => new SourceTypeCandidateParameter( destination.RequestedType, destination.Candidate.PropertyType ) ) );
		public static Func<Type, object> ServiceSource { get; } = GlobalServiceProvider.Default.GetService;
	}
}