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

	public class Source<T> : SourceBase<T>
	{
		readonly T instance;

		public Source( T instance )
		{
			this.instance = instance;
		}

		public override T Get() => instance;
	}
}