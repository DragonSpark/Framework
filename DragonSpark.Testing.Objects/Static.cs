namespace DragonSpark.Testing.Objects
{
	public class Static
	{
		public static void Assign<T>( T instance )
		{
			Instance = instance;
		}

		public static object Instance { get; private set; }
	}
}