namespace DragonSpark.Application.Security.Identity.Claims.Access;

public sealed class DisplayName : ReadClaim
{
	public static DisplayName Default { get; } = new();

	DisplayName() : base(Identity.DisplayName.Default) {}
}