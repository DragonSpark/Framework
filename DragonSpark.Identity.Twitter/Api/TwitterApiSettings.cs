namespace DragonSpark.Identity.Twitter.Api;

public sealed class TwitterApiSettings
{
	public string Key { get; set; } = default!;

	public string Secret { get; set; } = default!;

	public string Bearer { get; set; } = default!;
}