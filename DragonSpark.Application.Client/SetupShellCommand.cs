using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using Prism;
using Prism.Logging;
using Prism.Unity;
using System;
using System.Security.Principal;
using System.Windows;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Application.Client
{
	public class SetupShellCommand : Prism.Unity.Windows.SetupShellCommand
	{
		protected override void InitializeShell( SetupContext context )
		{
			base.InitializeShell( context );

			var application = System.Windows.Application.Current;
			Shell.As<Window>( window =>
			{
				context.Logger.Log( "Assigning and Displaying Primary Window/Shell.", Category.Info, Prism.Logging.Priority.None );
				application.MainWindow = window;
				window.Show();
				context.Logger.Log( "Primary Window/Shell Displayed.", Category.Info, Prism.Logging.Priority.None );
			} );

			context.Logger.Log( "Primary Window/Shell Initialized.", Category.Info, Prism.Logging.Priority.None );
		}
	}

	public class SetupApplicationCommand : SetupCommand
	{
		public PrincipalPolicy? PrincipalPolicy { get; set; }

		protected override void Execute( SetupContext context )
		{
			PrincipalPolicy.WithValue( AppDomain.CurrentDomain.SetPrincipalPolicy );

			var container = context.Container();

			System.Windows.Application.Current.With( x =>
			{
				container.RegisterInstance( x.Dispatcher );
				x.Exit += ( s, a ) => container.Resolve<IServiceLocator>().TryDispose();
			} );
		}
	}
}