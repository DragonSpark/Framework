using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity
{
	public sealed class DisplayName : Text.Text
	{
		public static DisplayName Default { get; } = new DisplayName();

		DisplayName() : base($"{ClaimNamespace.Default}:displayname") {}
	}

	public sealed class DefaultNameClaim : Text.Text
	{
		public static DefaultNameClaim Default { get; } = new DefaultNameClaim();

		DefaultNameClaim() : base(ClaimTypes.Name) {}
	}
}