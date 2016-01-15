using DragonSpark.Activation;
using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Setup;
using DragonSpark.Setup.Commands;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Threading.Tasks;
using System.Windows.Markup;
using IExceptionHandler = DragonSpark.Diagnostics.IExceptionHandler;

namespace DragonSpark.Windows.Setup
{
	[ContentProperty( nameof(Policies) )]
	public class SetupExceptionHandling : UnityCommand
	{
		[Activate]
		public IServiceRegistry Registry { get; set; }

		protected override void OnExecute( IApplicationSetupParameter parameter )
		{
			Registry.Register( new InstanceRegistrationParameter( new ExceptionManager( Policies ) ) );

			Container.TryResolve<IExceptionHandler>().With( ConfigureExceptionHandling );
		}
		
		protected virtual void ConfigureExceptionHandling( IExceptionHandler handler )
		{
			TaskScheduler.UnobservedTaskException += ( sender, args ) => handler.Process( args.Exception );
			AppDomain.CurrentDomain.With( x => x.UnhandledException += ( s, args ) =>
			{
				args.ExceptionObject.As<Exception>( handler.Process );
			} );
		}

		public Collection<Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyDefinition> Policies { get; } = new Collection<Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyDefinition>();
	}
}