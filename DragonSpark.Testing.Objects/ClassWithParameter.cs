namespace DragonSpark.Testing.Objects
{
	public class ClassWithParameter : IClassWithParameter, IInterface
	{
		public ClassWithParameter( object parameter )
		{
			Parameter = parameter;
		}

		public object Parameter { get; }
	}
}