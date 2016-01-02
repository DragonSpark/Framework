using DragonSpark.Activation;
using DragonSpark.Activation.IoC;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
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

		protected override void OnExecute( ISetupParameter parameter )
		{
			Extension.Enable( SetAsEnabled );
		}
	}

	public class SetupUnityCommand : ConfigureUnityCommand
	{
		[Activate]
		public IServiceLocation Location { get; set; }

		[Activate]
		public IServiceLocator Locator { get; set; }

		protected override void OnExecute( ISetupParameter parameter )
		{
			MessageLogger.Information( Resources.ConfiguringUnityContainer, Priority.Low );
			ConfigureContainer( parameter, Container );

			base.OnExecute( parameter );
		}

		protected virtual void ConfigureContainer( ISetupParameter parameter, IUnityContainer container )
		{
			container.Registration<EnsuredRegistrationSupport>().With( support =>
			{
				support.Instance( new ServiceLocationMonitor( Location, Locator ) );

				var objects = parameter.Append( parameter.Items ).Except( container.ToItem() );
				objects.Each( support.Convention );
			} );
		}
	}
}