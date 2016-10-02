namespace DragonSpark.Specifications
{
	public abstract class SpecificationWithContextBase<T> : SpecificationWithContextBase<T, T>
	{
		protected SpecificationWithContextBase( T context ) : base( context ) {}
	}
}