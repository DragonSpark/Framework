using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Text;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity;

sealed class UserName : IFormatter<ClaimsPrincipal>
{
	public static UserName Default { get; } = new();

	UserName() : this(Anonymous.Default) {}

	readonly string _anonymous;

	public UserName(string anonymous) => _anonymous = anonymous;

	public string Get(ClaimsPrincipal parameter)
		=> parameter.Identity?.IsAuthenticated ?? false
			   ? parameter.Identity?.Name ?? parameter.FindFirstValue(ClaimTypes.Name).Verify()
			   : _anonymous;
}