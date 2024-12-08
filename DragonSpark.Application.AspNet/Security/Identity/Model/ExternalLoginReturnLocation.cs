using Microsoft.AspNetCore.Mvc.Routing;

namespace DragonSpark.Application.AspNet.Security.Identity.Model;

sealed class ExternalLoginReturnLocation : ReturnLocation
{
	public ExternalLoginReturnLocation(IUrlHelperFactory factory)
		: base(factory, ExternalLoginReturnDefinition.Default) {}
}