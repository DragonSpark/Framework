namespace DragonSpark.Testing.TestObjects
{
	class Static
	{
		public static void Assign<T>( T instance )
		{
			Instance = instance;
		}

		public static object Instance { get; private set; }
	}
}