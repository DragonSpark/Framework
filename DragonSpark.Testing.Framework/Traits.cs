namespace DragonSpark.Testing.Framework
{
	public static class Traits
	{
		public const string Category = nameof(Category);

		public static class Categories
		{
			public const string FileSystem = nameof(FileSystem), IoC = nameof(IoC), ServiceLocation = nameof(ServiceLocation), Performance = nameof(Performance), Memory = nameof(Memory), Xaml = nameof(Xaml);
		}
	}
}