using DragonSpark.Extensions;
using DragonSpark.Specifications;
using System.Collections.Immutable;
using System.Reflection;

namespace DragonSpark.Activation.Location
{
	public class SingletonSpecification : SpecificationBase<PropertyInfo>
	{
		public static SingletonSpecification Default { get; } = new SingletonSpecification();
		SingletonSpecification() : this( "Instance", "Default", "DefaultNested" ) {}

		readonly ImmutableArray<string> candidates;

		public SingletonSpecification( params string[] candidates ) : this( candidates.ToImmutableArray() ) {}

		public SingletonSpecification( ImmutableArray<string> candidates )
		{
			this.candidates = candidates;
		}

		public override bool IsSatisfiedBy( PropertyInfo parameter )
		{
			var result = parameter.GetMethod.IsStatic && !parameter.GetMethod.ContainsGenericParameters 
				&& 
				( candidates.Contains( parameter.Name ) || parameter.Has<SingletonAttribute>() );
			return result;
		}
	}
}