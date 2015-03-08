using System;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using DragonSpark.Application.Presentation.Security;
using Microsoft.LightSwitch.Security;
using Microsoft.LightSwitch.Security.Client;

namespace DragonSpark.Application.Presentation
{
	public class AuthenticationServiceBridge : FormsAuthentication, IAuthenticationService
	{
		static readonly IUser DefaultUser = new ServiceUser( Application.AuthenticationType, false, "Anonymous", Enumerable.Empty<string>(), Enumerable.Empty<string>() );

		public new event EventHandler LoggedIn = delegate { };

		public IAuthenticationOperation LoadUser( Action<IAuthenticationOperation> callback, object userState )
		{
			throw new NotSupportedException( "This implementation is for application initialization only." );
		}

		public IAuthenticationOperation Login( string userName, string password, Action<IAuthenticationOperation> callback, object userState )
		{
			throw new NotSupportedException( "This implementation is for application initialization only." );
		}

		public IAuthenticationOperation ResolveAuthenticationType( Action<IAuthenticationOperation> callback, object userState )
		{
			Continue = callback;
			return null;
		}

		Action<IAuthenticationOperation> Continue { get; set; }

		protected override IPrincipal CreateDefaultUser()
		{
			return DefaultUser;
		}

		IUser IAuthenticationService.User
		{
			get { return User as IUser; }
		}

		public AuthenticationType AuthenticationType
		{
			get { return Application.AuthenticationType; }
		}

		public void Run()
		{
			var operation = new ResolveAuthenticationTypeOperation( User, AuthenticationType.Windows, null );
			Continue( operation );
			LoggedIn( this, EventArgs.Empty );
		}
		
		abstract class AuthenticationOperationBase : IAuthenticationOperation
		{
			// Fields
			// readonly OperationBase operation;
			readonly object userState;

			// Methods
			protected AuthenticationOperationBase( object userState )
			{
				this.userState = userState;
			}

			// Properties
			public abstract AuthenticationType AuthenticationType { get; }

			public virtual Exception Exception
			{
				get { return null; }
			}

			public abstract IPrincipal Principal { get; }

			public object UserState
			{
				get { return userState; }
			}
		}

		class ResolveAuthenticationTypeOperation : AuthenticationOperationBase
		{
			// Fields
			readonly IPrincipal principal;
			readonly AuthenticationType authenticationType;

			public ResolveAuthenticationTypeOperation( IPrincipal principal, AuthenticationType authenticationType, object userState ) : base( userState )
			{
				this.principal = principal;
				this.authenticationType = authenticationType;
			}

			// Properties
			public override AuthenticationType AuthenticationType
			{
				get { return authenticationType; }
			}

			public override IPrincipal Principal
			{
				get { return principal; }
			}
		}
	}
}