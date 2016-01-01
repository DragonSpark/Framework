namespace DragonSpark.Testing.Objects
{
	[Attribute( PropertyName = "This is a class attribute." )]
	public class Decorated
	{
		[Attribute( PropertyName = "This is a property attribute." )]
		public string Property { get; set; }
	}
}