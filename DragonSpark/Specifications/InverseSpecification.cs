namespace DragonSpark.Specifications
{
	public class InverseSpecification : InverseSpecification<object>
	{
		public InverseSpecification( ISpecification<object> inner ) : base( inner ) {}
	}
}