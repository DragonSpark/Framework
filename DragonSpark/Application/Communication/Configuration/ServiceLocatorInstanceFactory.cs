using DragonSpark.Configuration;
using DragonSpark.Objects;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Application.Communication.Configuration
{
	public class ServiceLocatorInstanceFactory<TLocator> : Factory<IServiceLocator> where TLocator : IInstanceSource<IServiceLocator>, new()
	{
		protected override IServiceLocator CreateItem(object source)
		{
			var provider = new TLocator();
			var result = provider.Instance;
			return result;
		}
	}
}