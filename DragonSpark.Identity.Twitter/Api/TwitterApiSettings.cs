namespace DragonSpark.Identity.Twitter.Api;

public sealed class TwitterApiSettings
{
	public string Key { get; set; } = null!;

	public string Secret { get; set; } = null!;

	public string Bearer { get; set; } = null!;
}