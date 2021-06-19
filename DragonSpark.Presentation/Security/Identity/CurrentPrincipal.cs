using DragonSpark.Application.Security.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace DragonSpark.Presentation.Security.Identity
{
	sealed class CurrentPrincipal : ICurrentPrincipal
	{
		readonly IHttpContextAccessor _accessor;

		public CurrentPrincipal(IHttpContextAccessor accessor) => _accessor = accessor;

		public ClaimsPrincipal Get() => _accessor.HttpContext?.User ?? throw new InvalidOperationException();
	}
}