using DragonSpark.Compose;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security;

sealed class CurrentContext : ICurrentContext
{
	readonly IHttpContextAccessor _accessor;

	public CurrentContext(IHttpContextAccessor accessor) => _accessor = accessor;

	public HttpContext Get() => _accessor.HttpContext.Verify();
}