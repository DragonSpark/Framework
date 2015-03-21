using DragonSpark.Application.Setup;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Prism;
using Prism.Logging;
using Prism.Modularity;
using Prism.Unity;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Input;
using System.Windows.Markup;
using RegisterFrameworkExceptionTypesCommand = Prism.Unity.RegisterFrameworkExceptionTypesCommand;
using ServiceLocator = DragonSpark.Activation.IoC.ServiceLocator;

namespace DragonSpark.Application
{
	public class Setup<TLoggingFacade> : Setup<TLoggingFacade, AssemblyModuleCatalog> where TLoggingFacade : ILoggerFacade, new()
	{}

	[ContentProperty( "Commands" )]
	public class Setup<TLoggingFacade, TModuleCatalog> : Prism.Setup<TLoggingFacade, TModuleCatalog> where TLoggingFacade : ILoggerFacade, new() where TModuleCatalog : IModuleCatalog, new()
	{
		protected override IList<ICommand> DetermineDefaultCommands()
		{
			var result = base.DetermineDefaultCommands();
			result.AddRange( new ICommand[]
			{
				new SetupUnityCommand(), new RegisterFrameworkExceptionTypesCommand()
			} );
			return result;
		}

		public override void Run( object arguments = null )
		{
			Events.Publish<SetupEvent, SetupStatus>( SetupStatus.Configuring );

			base.Run( arguments );

			Events.Publish<SetupEvent, SetupStatus>( SetupStatus.Configured );
		}

		protected override ICommand Prepare( ICommand command )
		{
			return command.WithDefaults();
		}
	}

	public class SetupUnityCommand : Prism.Unity.Windows.SetupUnityCommand
	{
		protected override IUnityContainer CreateContainer()
		{
			var locator = CreateServiceLocator();
			var result = locator.GetInstance<IUnityContainer>();
			return result;
		}

		protected virtual IServiceLocator CreateServiceLocator()
		{
			var container = base.CreateContainer();
			var result = new ServiceLocator( container );
			return result;
		}
	}

	public class SetupUnityFromConfigurationCommand : SetupCommand
	{
		protected override void Execute( SetupContext context )
		{
			ConfigurationManager.GetSection( "unity" ).As<UnityConfigurationSection>( x => x.Containers.Any().IsTrue( () => context.Container().LoadConfiguration() ) );
		}
	}
}