namespace DragonSpark.Testing.TestObjects
{
	[Attribute( PropertyName = "This is a class attribute through convention." )]
	class ConventionMetadata
	{
		[Attribute( PropertyName = "This is a property attribute through convention." )] 
		public string Property { get; set; }
	}
}