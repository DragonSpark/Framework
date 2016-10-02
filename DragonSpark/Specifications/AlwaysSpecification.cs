namespace DragonSpark.Specifications
{
	public class Common<T>
	{
		protected Common() {}

		// protected Common() {}

		public static ISpecification<T> Assigned { get; } = AssignedSpecification<T>.Default;
		
		public static ISpecification<T> Always { get; } = new SuppliedSpecification<T>( true );

		public static ISpecification<T> Never { get; } = new SuppliedSpecification<T>( false );
	}
}