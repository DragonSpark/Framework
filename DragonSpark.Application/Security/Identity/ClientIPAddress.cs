using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.Security.Identity
{
	public sealed class ClientIPAddress : IResult<System.Net.IPAddress>
	{
		readonly IHttpContextAccessor _accessor;

		public ClientIPAddress(IHttpContextAccessor accessor) => _accessor = accessor;

		public System.Net.IPAddress Get() => _accessor.HttpContext.Verify().Connection.RemoteIpAddress.Verify();
	}
}