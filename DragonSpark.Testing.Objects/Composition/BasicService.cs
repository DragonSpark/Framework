namespace DragonSpark.Testing.Objects.Composition
{
	public class BasicService : IBasicService
	{
		public string HelloWorld( string message ) => $"Hello there! {message}";
	}
}
