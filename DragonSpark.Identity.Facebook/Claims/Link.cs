using JetBrains.Annotations;

namespace DragonSpark.Identity.Facebook.Claims;

public sealed class Link : FacebookClaim
{
	[UsedImplicitly]
	public static Link Default { get; } = new();

	Link() : base(nameof(Link).ToLowerInvariant()) {}
}