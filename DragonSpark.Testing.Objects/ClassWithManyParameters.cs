namespace DragonSpark.Testing.Objects
{
	public class ClassWithManyParameters
	{
		public ClassWithManyParameters( string @string, int integer, Class @class )
		{
			String = @string;
			Integer = integer;
			Class = @class;
		}

		public string String { get; }

		public int Integer { get; }

		public Class Class { get; }
	}
}