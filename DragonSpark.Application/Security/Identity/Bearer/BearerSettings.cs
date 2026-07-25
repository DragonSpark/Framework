namespace DragonSpark.Application.Security.Identity.Bearer;

public sealed record BearerSettings
{
	public required string Key { get; set; }

	public required string Issuer { get; set; }

	public required string Audience { get; set; }

	public TimeSpan Window { get; set; } = TimeSpan.FromHours(1);
}