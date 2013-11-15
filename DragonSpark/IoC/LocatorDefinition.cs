using DragonSpark.Configuration;
using DragonSpark.Extensions;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System.Collections.ObjectModel;
using System.Windows.Markup;

namespace DragonSpark.IoC
{
	[ContentProperty( "Configurations" )]
	public class LocatorDefinition : Singleton<IServiceLocator>
	{
		public Collection<IContainerConfigurationCommand> Configurations
		{
			get { return configurations; }
		}	readonly Collection<IContainerConfigurationCommand> configurations = new Collection<IContainerConfigurationCommand>();
		
		protected override IServiceLocator Create()
		{
			var container = new UnityContainer().AddNewExtension<DragonSparkExtension>();
			var result = new UnityServiceLocator( container );
			Configurations.Apply( x => x.Configure( container ) );
			container.Configure<DragonSparkExtension>().Complete();
			return result;
		}
	}
}