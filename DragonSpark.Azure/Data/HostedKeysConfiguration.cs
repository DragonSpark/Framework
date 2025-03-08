namespace DragonSpark.Azure.Data;

public sealed class HostedKeysConfiguration
{
	public string Location { get; set; } = null!;

	public string Vault { get; set; } = null!;
}