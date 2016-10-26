namespace DragonSpark.Sources
{
	public abstract class SourceBase<T> : ISource<T>
	{
		public abstract T Get();

		object ISource.Get() => Get();
	}
}