using DragonSpark.Activation;
using DragonSpark.Activation.IoC;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Modularity;
using DragonSpark.Properties;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;
using System.ComponentModel;
using System.Linq;

namespace DragonSpark.Setup.Commands
{
	public class SetupObjectBuilderCommand : SetupCommand
	{
		[Extension]
		public ObjectBuilderExtension Extension { [return: NotNull]get; set; }

		[DefaultValue( true )]
		public bool SetAsEnabled { get; set; }

		protected override void Execute( SetupContext context )
		{
			Extension.Enable( SetAsEnabled );
		}
	}

	public class SetupUnityCommand : SetupCommand
	{
		[Default( true )]
		public bool UseDefaultConfiguration { get; set; }

		[Activate]
		public IUnityContainer Container { [return: NotNull]get; set; } // .InvalidIfNull( Resources.NullUnityContainerException )

		[Activate]
		public IServiceLocation Location { get; set; }

		[Activate]
		public IServiceLocator Locator { get; set; }

		protected override void Execute( SetupContext context )
		{
			context.Register( Container );

			context.MessageLogger.Information( Resources.ConfiguringUnityContainer, Priority.Low );
			ConfigureContainer( context, Container );
		}

		protected virtual void ConfigureContainer( SetupContext context, IUnityContainer container )
		{
			container.Registration<EnsuredRegistrationSupport>().With( support =>
			{
				support.Instance( new ServiceLocationMonitor( Location, Locator ) );

				support.AllInterfaces( context.MessageLogger );

				var objects = context.Items.Except( container.ToItem() );
				objects.Each( support.Convention );

				if ( UseDefaultConfiguration )
				{
					support.Mapping<IModuleInitializer, ModuleInitializer>( new ContainerControlledLifetimeManager() );
					support.Mapping<IModuleManager, ModuleManager>( new ContainerControlledLifetimeManager() );
				}
			} );
		}
	}
}