using DragonSpark.Configuration;

namespace DragonSpark.Objects.Configuration
{
	public abstract class ObjectFinderBase : IInstanceSource<ILocator>
	{
		protected abstract ILocator Create();
		
		public ILocator Instance
		{
			get { return instance ?? ( instance = Create() ); }
		}	ILocator instance;
	}
}