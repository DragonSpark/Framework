using System.ServiceModel.DomainServices.Client;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using DragonSpark.Extensions;
using DragonSpark.Objects;

namespace DragonSpark.Application.Presentation.Entity.Security
{
	public class SignOutOperation : AuthenticationOperationCommandBase
	{
		readonly string cookieName;

		public SignOutOperation( AuthenticationService authenticationService, string cookieName = "FedAuth" ) : base( authenticationService )
		{
			this.cookieName = cookieName;
		}

		protected override OperationBase ResolveOperation()
		{
			var result = AuthenticationService.Logout( true );
			return result;
		}

		protected override void MarkComplete( OperationBase operation )
		{
			base.MarkComplete( operation );
			Exception.Null( () => Cookie.DeleteCookie( cookieName ) );
		}

		[DefaultPropertyValue( "Signing Out Current User..." )]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}
	}
}