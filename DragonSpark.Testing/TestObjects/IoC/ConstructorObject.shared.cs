namespace DragonSpark.Testing.TestObjects.IoC
{
	public class ConstructorObject
	{
		readonly int number;
		readonly string message;

		public ConstructorObject( string property )
		{
			this.message = property;
		}

		public ConstructorObject( int number )
		{
			this.number = number;
		}

		public int Number
		{
			get { return number; }
		}

		public string Message
		{
			get { return message; }
		}
	}
}