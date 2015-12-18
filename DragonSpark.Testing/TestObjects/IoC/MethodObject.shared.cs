namespace DragonSpark.Testing.TestObjects.IoC
{
	public class MethodObject
	{
		public string Message { get; set; }

		public void Method( string message )
		{
			Message = message;
		}
	}
}