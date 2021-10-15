using Microsoft.AspNetCore.Mvc.Routing;

namespace DragonSpark.Application.Security.Identity.Model;

sealed class ReturnOrRoot : ReturnLocation
{
	public ReturnOrRoot(IUrlHelperFactory factory) : base(factory, ReturnOrRootDefinition.Default) {}
}