using DragonSpark.Application.AspNet.Security.Identity.Claims.Access;

namespace DragonSpark.Application.AspNet.Security.Identity;

public sealed class DisplayName : Text.Text
{
	public static DisplayName Default { get; } = new();

	DisplayName() : base($"{ClaimNamespace.Default}:displayname") {}
}