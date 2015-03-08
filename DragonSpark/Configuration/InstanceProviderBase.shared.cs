namespace DragonSpark.Configuration
{
	public abstract class InstanceSourceBase<TItem> : IInstanceSource<TItem> where TItem : class
	{
		public TItem Instance
		{
			get { return instance ?? ( instance = Create() ); }
		}	TItem instance;

		protected abstract TItem Create();
	}
}