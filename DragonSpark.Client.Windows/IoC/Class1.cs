using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Security.Principal;

namespace DragonSpark.Client.Windows.IoC
{
	public class ApplicationConfigurationCommand : Common.IoC.Commands.ApplicationConfigurationCommand
	{
		// [Default( PrincipalPolicy.WindowsPrincipal )]
		public PrincipalPolicy? PrincipalPolicy { get; set; }

		protected override void OnConfigure( IUnityContainer container )
		{
			base.OnConfigure( container );

			PrincipalPolicy.With( policy =>
			{
				AppDomain.CurrentDomain.SetPrincipalPolicy( policy.Value );
			} );

			System.Windows.Application.Current.With( x =>
			{
				container.RegisterInstance( System.Windows.Application.Current.Dispatcher );
				x.Exit += ( s, a ) => container.Resolve<IServiceLocator>().TryDispose();
			} );
		}
	}
}
