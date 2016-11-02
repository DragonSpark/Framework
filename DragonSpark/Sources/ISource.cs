namespace DragonSpark.Sources
{
	public interface ISource<out T> : ISource
	{
		new T Get();
	}

	public interface ISource
	{
		object Get();
	}
}