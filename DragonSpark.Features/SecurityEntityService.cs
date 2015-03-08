using System.ServiceModel.DomainServices.Hosting;
using DragonSpark.Application.Communication.Security;
using DragonSpark.Features.Entity;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;

namespace DragonSpark.Features
{
	[EnableClientAccess, ExceptionShielding]
	public class SecurityEntityService : SecurityServiceBase<FeaturesEntityStorage, User>
	{}

	// [ServiceBehavior(IncludeExceptionDetailInFaults = true, AddressFilterMode = AddressFilterMode.Any)]
}