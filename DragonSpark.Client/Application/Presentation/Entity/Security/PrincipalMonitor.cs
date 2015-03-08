using System;
using System.ComponentModel;
using System.Security.Principal;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.IoC;

namespace DragonSpark.Application.Presentation.Entity.Security
{
	[Singleton( typeof(INotifyPrincipalChanged), Priority = Priority.Lowest )]
	[Singleton( typeof(IPrincipal), Priority = Priority.Lowest )]
	public class PrincipalMonitor : ViewObject, INotifyPrincipalChanged
	{
		public event EventHandler<PrincipalChangedEventArgs> PrincipalChanged = delegate {};

		public PrincipalMonitor( AuthenticationService service )
		{
			InnerPrincipal = service.User;
			service.As<INotifyPropertyChanged>( item => item.PropertyChanged += ( s, a ) =>
			{
			    switch ( a.PropertyName )
			    {
			        case "IsLoggingIn":
			        case "IsLoggingOut":
			        case "IsLoadingUser":
					case "User":
			            if ( !service.IsBusy && InnerPrincipal.Transform( y => y.Identity.Transform( z => z.IsAuthenticated ) ) != service.User.Transform( y => y.Identity.Transform( z => z.IsAuthenticated ) ) )
			            {
			                OnPrincipalChanged( this, new PrincipalChangedEventArgs( service.User ) );
			            }
			            break;
			    }
			} );
		}

		void OnPrincipalChanged(object sender, PrincipalChangedEventArgs e)
		{
			InnerPrincipal = e.Principal;
			PrincipalChanged( sender, e );
		}

		IPrincipal InnerPrincipal
		{
			get { return innerPrincipal; }
			set
			{
				if ( SetProperty( ref innerPrincipal, value, () => InnerPrincipal ) )
				{
					Identity = value.Transform( x => x.Identity );
				}
			}
		}	IPrincipal innerPrincipal;

		public IIdentity Identity
		{
			get { return identity; }
			private set { SetProperty( ref identity, value, () => Identity ); }
		}	IIdentity identity;

		public bool IsInRole( string role )
		{
			var result = InnerPrincipal.IsInRole( role );
			return result;
		}
	}
}