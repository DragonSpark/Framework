using DragonSpark.Application.Presentation.Entity.Operations;
using System.ServiceModel.DomainServices.Client.ApplicationServices;

namespace DragonSpark.Application.Presentation.Entity.Security
{
	public abstract class AuthenticationOperationCommandBase : EntityOperationCommmandBase
	{
		readonly AuthenticationService authenticationService;

		protected AuthenticationOperationCommandBase( AuthenticationService authenticationService )
		{
			this.authenticationService = authenticationService;
		}

		protected AuthenticationService AuthenticationService
		{
			get { return authenticationService; }
		}
	}
}