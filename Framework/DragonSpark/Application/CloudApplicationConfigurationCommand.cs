using System.Configuration;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace DragonSpark.Application
{
    public class CloudApplicationConfigurationCommand : IContainerConfigurationCommand
	{
		public bool EnableRoleConfiguration { get; set; }

		public void Configure( IUnityContainer container )
		{
			CloudStorageAccount.SetConfigurationSettingPublisher( (configName, configSetter) =>
			{
				var setting = EnableRoleConfiguration && RoleEnvironment.IsAvailable ? RoleEnvironment.GetConfigurationSettingValue( configName ) : ConfigurationManager.AppSettings[ configName ];
				configSetter( setting );
			} );
		}
	}
}
