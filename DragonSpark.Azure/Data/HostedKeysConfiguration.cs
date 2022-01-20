namespace DragonSpark.Azure.Data;

public sealed class HostedKeysConfiguration
{
	public string Location { get; set; } = default!;

	public string Vault { get; set; } = default!;
}