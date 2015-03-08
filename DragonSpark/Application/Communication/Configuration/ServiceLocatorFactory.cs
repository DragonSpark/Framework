using DragonSpark.Objects;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Application.Communication.Configuration
{
	public class ServiceLocatorFactory<TLocator> : Factory<IServiceLocator> where TLocator : IServiceLocator, new()
	{
		protected override IServiceLocator CreateItem( object source )
		{
			var result = new TLocator();
			return result;
		}
	}
}