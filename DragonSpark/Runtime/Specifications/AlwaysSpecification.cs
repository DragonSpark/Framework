namespace DragonSpark.Runtime.Specifications
{
	public class AlwaysSpecification : FixedSpecification
	{
		public static AlwaysSpecification Instance { get; } = new AlwaysSpecification();

		AlwaysSpecification() : base( true )
		{}
	}
}