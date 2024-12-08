using Microsoft.AspNetCore.Mvc.Routing;

namespace DragonSpark.Application.AspNet.Security.Identity.Model;

sealed class ReturnOrRoot : ReturnLocation
{
	public ReturnOrRoot(IUrlHelperFactory factory) : base(factory, ReturnOrRootDefinition.Default) {}
}