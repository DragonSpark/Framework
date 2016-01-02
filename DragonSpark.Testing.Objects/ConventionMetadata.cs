namespace DragonSpark.Testing.Objects
{
	[Attribute( PropertyName = "This is a class attribute through convention." )]
	public class ConventionMetadata
	{
		[Attribute( PropertyName = "This is a property attribute through convention." )] 
		public string Property { get; set; }
	}
}