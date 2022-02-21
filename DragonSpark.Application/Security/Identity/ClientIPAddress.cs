using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Security.Identity;

public sealed class ClientIPAddress : IResult<System.Net.IPAddress>
{
	readonly ICurrentContext _accessor;

	public ClientIPAddress(ICurrentContext accessor) => _accessor = accessor;

	public System.Net.IPAddress Get() => _accessor.Get().Connection.RemoteIpAddress.Verify();
}