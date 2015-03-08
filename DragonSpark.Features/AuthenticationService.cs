using System.ServiceModel.DomainServices.Hosting;
using DragonSpark.Application.Communication.Security;
using DragonSpark.Features.Entity;

namespace DragonSpark.Features
{
	// [ServiceBehavior(IncludeExceptionDetailInFaults = true, AddressFilterMode = AddressFilterMode.Any)]
	[EnableClientAccess]
	public class AuthenticationService : AuthenticationServiceBase<FeaturesEntityStorage, User>
	{}
}