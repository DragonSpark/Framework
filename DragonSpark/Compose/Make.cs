namespace DragonSpark.Compose
{
	public static class Make
	{
		public static T A<T>() => Start.An.Instance<T>();
	}
}