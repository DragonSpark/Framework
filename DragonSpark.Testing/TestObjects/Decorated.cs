namespace DragonSpark.Testing.TestObjects
{
	[Attribute( PropertyName = "This is a class attribute." )]
	class Decorated
	{
		[Attribute( PropertyName = "This is a property attribute." )]
		public string Property { get; set; }
	}
}