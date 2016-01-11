namespace DragonSpark.Runtime.Specifications
{
	public class IsTypeSpecification<T> : SpecificationBase<T>
	{
		public static IsTypeSpecification<T> Instance { get; } = new IsTypeSpecification<T>();
	}
}