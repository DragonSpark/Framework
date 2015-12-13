namespace DragonSpark.Runtime.Specifications
{
	public class NeverSpecification : FixedSpecification
	{
		public static NeverSpecification Instance { get; } = new NeverSpecification();

		NeverSpecification() : base( false )
		{}
	}
}