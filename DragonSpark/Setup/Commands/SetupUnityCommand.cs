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

	public class SetupUnityCommand : ConfigureUnityCommand
	{
		[Activate]
		public IServiceLocation Location { get; set; }

		[Activate]
		public IServiceLocator Locator { get; set; }

		protected override void Execute( SetupContext context )
		{
			MessageLogger.Information( Resources.ConfiguringUnityContainer, Priority.Low );
			ConfigureContainer( context, Container );

			base.Execute( context );
		}

		protected virtual void ConfigureContainer( SetupContext context, IUnityContainer container )
		{
			container.Registration<EnsuredRegistrationSupport>().With( support =>
			{
				support.Instance( new ServiceLocationMonitor( Location, Locator ) );

				var objects = context.Append( context.Items ).Except( container.ToItem() );
				objects.Each( support.Convention );
			} );
		}
	}
}