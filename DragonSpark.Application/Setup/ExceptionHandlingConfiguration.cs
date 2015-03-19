using DragonSpark.Extensions;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.Unity;
using Prism;
using Prism.Unity;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Markup;
using IExceptionHandler = DragonSpark.Diagnostics.IExceptionHandler;

namespace DragonSpark.Application.Setup
{
	[ContentProperty( "Policies" )]
	public class ExceptionHandlingConfiguration : SetupCommand
	{

		protected override void Execute( SetupContext context )
		{
			ExceptionPolicy.Reset();

			var manager = new ExceptionManager( Policies );
			var container = context.Container();
			container.RegisterInstance( manager );

			ExceptionPolicy.SetExceptionManager( manager );

			var exceptionHandler = container.TryResolve<IExceptionHandler>();
			exceptionHandler.NotNull( ConfigureExceptionHandling );
		}
		
		protected virtual void ConfigureExceptionHandling( IExceptionHandler handler )
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