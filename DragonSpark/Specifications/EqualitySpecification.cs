using JetBrains.Annotations;

namespace DragonSpark.Specifications
{
	[UsedImplicitly]
	public class EqualitySpecification : EqualitySpecification<object>
	{
		public EqualitySpecification( object context ) : base( context ) {}
	}

	public class EqualitySpecification<T> : SpecificationWithContextBase<T>
	{
		public EqualitySpecification( T context ) : base( context ) {}

		public override bool IsSatisfiedBy( T parameter ) => Equals( Context, parameter );
	}
}