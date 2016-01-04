namespace DragonSpark.TypeSystem
{
	public static class Default<T>
	{
		public static T Item => DefaultFactory<T>.Instance.Create();
		public static T[] Items => DefaultFactory<T[]>.Instance.Create();
	}
}