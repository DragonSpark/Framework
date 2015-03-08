using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Configuration
{
	public class ConfigureContainerFromConfigurationCommand : IContainerConfigurationCommand
	{
		public void Configure( IUnityContainer container )
		{
			using (var configurator = new UnityContainerConfigurator(container))
			{
				using ( var source = ConfigurationSourceFactory.Create() )
				{
					EnterpriseLibraryContainer.ConfigureContainer( configurator, source );
				}
			}
		}
	}
}