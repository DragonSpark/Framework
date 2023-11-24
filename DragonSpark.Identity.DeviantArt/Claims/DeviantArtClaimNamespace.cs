namespace DragonSpark.Identity.DeviantArt.Claims;

public sealed class DeviantArtClaimNamespace : Text.Text
{
	public static DeviantArtClaimNamespace Default { get; } = new();

	DeviantArtClaimNamespace() : base("urn:deviantart") {}
}