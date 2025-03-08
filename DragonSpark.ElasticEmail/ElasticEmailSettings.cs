namespace DragonSpark.ElasticEmail;

public sealed class ElasticEmailSettings
{
	public string FromAddress { get; set; } = null!;

	public string FromName { get; set; } = null!;

	public string ApiKey { get; set; } = null!;
}