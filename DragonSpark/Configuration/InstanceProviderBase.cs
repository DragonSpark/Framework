namespace DragonSpark.Configuration
{
	public abstract class Singleton<TItem> : IInstanceSource<TItem> where TItem : class
	{
		public TItem Instance
		{
			get { return instance ?? ( instance = Create() ); }
		}	TItem instance;

		protected abstract TItem Create();
	}
}