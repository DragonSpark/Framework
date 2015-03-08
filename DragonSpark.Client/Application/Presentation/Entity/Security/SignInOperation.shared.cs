using System.ServiceModel.DomainServices.Client;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using DragonSpark.Objects;

namespace DragonSpark.Application.Presentation.Entity.Security
{
	public class SignInOperation : AuthenticationOperationCommandBase
	{
		public SignInOperation( AuthenticationService authenticationService ) : base( authenticationService )
		{}

		protected override OperationBase ResolveOperation()
		{
			var result = AuthenticationService.Login( string.Empty, string.Empty );
			return result;
		}

		[DefaultPropertyValue( "Signing In..." )]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}
	}
}