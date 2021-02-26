namespace DragonSpark.ElasticEmail
{
	public sealed class ElasticEmailSettings
	{
		public string FromAddress { get; set; } = default!;

		public string FromName { get; set; } = default!;

		public string ApiKey { get; set; } = default!;
	}
}