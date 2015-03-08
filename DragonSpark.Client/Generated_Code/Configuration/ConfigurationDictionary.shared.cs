using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Configuration
{
	public class ConfigurationDictionary : ConfigurationDictionary<IServiceLocator>
	{
		protected override IServiceLocator ResolveInstance()
		{
			return Locator;
		}
	}
}