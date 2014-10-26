namespace DragonSpark.Testing.TestObjects
{
	public class ClassWithParameter : IClassWithParameter, IInterface
	{
		public object Parameter { get; set; }

		public ClassWithParameter( object parameter )
		{
			Parameter = parameter;
		}
	}
}