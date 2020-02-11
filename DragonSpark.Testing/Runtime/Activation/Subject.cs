namespace DragonSpark.Testing.Runtime.Activation
{
	sealed class Subject
	{
		public static Subject Default { get; } = new Subject();

		Subject() {}
	}
}