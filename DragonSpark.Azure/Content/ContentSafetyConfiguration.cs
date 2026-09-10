namespace DragonSpark.Azure.Content;

public sealed class ContentSafetyConfiguration
{
	public required Uri Endpoint { get; set; }

	public required string Key { get; set; }

	public byte AllowedSafety { get; set; }
}