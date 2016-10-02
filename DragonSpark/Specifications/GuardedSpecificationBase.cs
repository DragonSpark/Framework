namespace DragonSpark.Specifications
{
	public class InverseSpecification<T> : DecoratedSpecification<T>
	{
		public InverseSpecification( ISpecification<T> inner ) : base( inner ) {}

		public override bool IsSatisfiedBy( T parameter ) => !base.IsSatisfiedBy( parameter );
	}
}