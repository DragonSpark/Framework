namespace DragonSpark.Entra;

public sealed record EntraApplicationSettings
{
	public required string Instance { get; set; } = "https://login.microsoftonline.com/";
	public required string TenantId { get; set; }
	public required string ClientId { get; set; }
	public required string ClientSecret { get; set; }
	public required string CallbackPath { get; set; } = "/signin-oidc";
	public required string ResponseType { get; set; } = "id_token";
}