namespace DragonSpark.Runtime.Specifications
{
	public class IsTypeSpecification<T> : SpecificationBase<object>
	{
		public static IsTypeSpecification<T> Instance { get; } = new IsTypeSpecification<T>();

		protected override bool IsSatisfiedByParameter( object parameter ) => parameter is T;
	}
}