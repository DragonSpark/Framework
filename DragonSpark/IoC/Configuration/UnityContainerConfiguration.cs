using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Markup;

namespace DragonSpark.IoC.Configuration
{
	[ContentProperty( "Types" )]
	public class UnityContainerConfiguration : IContainerConfigurationCommand
	{
		protected virtual void Configure( IUnityContainer container )
		{
			// Add extensions:
			Extensions.Apply( x => container.AddExtension( x ) );
			
			// Go through commands and configuration:
			var commands = PreConfigurations.Concat( Instances ).Concat( Configurations ).Concat( Types ).Concat( PostConfigurations ).ToArray();
			commands.Apply( item => item.Configure( container ) );
		}

		void IContainerConfigurationCommand.Configure( IUnityContainer container )
		{
			Configure( container );
		}

		public Collection<UnityContainerExtension> Extensions
		{
			get { return extensions; }
		}	readonly Collection<UnityContainerExtension> extensions = new Collection<UnityContainerExtension>();

		public Collection<UnityType> Types
		{
			get { return types; }
		}	readonly Collection<UnityType> types = new Collection<UnityType>();

		public Collection<UnityInstance> Instances
		{
			get { return instances; }
		}	readonly Collection<UnityInstance> instances = new Collection<UnityInstance>();

		public Collection<IContainerConfigurationCommand> PreConfigurations
		{
			get { return preConfigurations; }
		}	readonly Collection<IContainerConfigurationCommand> preConfigurations = new Collection<IContainerConfigurationCommand>();

		public Collection<IContainerConfigurationCommand> Configurations
		{
			get { return configurations; }
		}	readonly Collection<IContainerConfigurationCommand> configurations = new Collection<IContainerConfigurationCommand>();

		public Collection<IContainerConfigurationCommand> PostConfigurations
		{
			get { return postConfigurations; }
		}	readonly Collection<IContainerConfigurationCommand> postConfigurations = new Collection<IContainerConfigurationCommand>();

	}
}