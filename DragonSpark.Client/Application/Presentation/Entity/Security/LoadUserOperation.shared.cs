using System.ServiceModel.DomainServices.Client;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using DragonSpark.Objects;

namespace DragonSpark.Application.Presentation.Entity.Security
{
	public class LoadUserOperation : AuthenticationOperationCommandBase
	{
		public LoadUserOperation( AuthenticationService authenticationService ) : base( authenticationService )
		{}

		protected override OperationBase ResolveOperation()
		{
			var result = AuthenticationService.LoadUser();
			return result;
		}

		[DefaultPropertyValue( "Refreshing Current User" )]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}
	}
}