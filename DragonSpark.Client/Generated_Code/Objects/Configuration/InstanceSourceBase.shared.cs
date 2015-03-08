using DragonSpark.Configuration;

namespace DragonSpark.Objects.Configuration
{
	public abstract class InstanceSourceBase : IInstanceSource<IFactory>
	{
		public IFactory Instance
		{
			get { return instance ?? ( instance = Create() ); }
		}	IFactory instance;

		protected abstract IFactory Create();
	}
}