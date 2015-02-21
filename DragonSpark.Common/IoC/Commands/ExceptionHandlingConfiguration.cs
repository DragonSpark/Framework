using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Markup;
using DragonSpark.Activation.IoC;
using DragonSpark.Extensions;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

namespace DragonSpark.Common.IoC.Commands
{
	[ContentProperty( "Policies" )]
	public class ExceptionHandlingConfiguration : IContainerConfigurationCommand
	{
		public void Configure( IUnityContainer container )
		{
			ExceptionPolicy.Reset();

			var manager = new ExceptionManager( Policies );
			container.RegisterInstance( manager );

			ExceptionPolicy.SetExceptionManager( manager );

			var exceptionHandler = container.TryResolve<Diagnostics.IExceptionHandler>();
			exceptionHandler.NotNull( ConfigureExceptionHandling );
		}

		protected virtual void ConfigureExceptionHandling( Diagnostics.IExceptionHandler handler )
		{
			TaskScheduler.UnobservedTaskException += ( sender, args ) => handler.Process( args.Exception );
			AppDomain.CurrentDomain.With( x => x.UnhandledException += ( s, args ) =>
			{
				args.ExceptionObject.As<Exception>( handler.Process );
			} );
		}

		public Collection<Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyDefinition> Policies
		{
			get { return policies; }
		}	readonly Collection<Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyDefinition> policies = new Collection<Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyDefinition>();
	}
}