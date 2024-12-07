namespace DragonSpark.Application.Entities.Initialization;

public sealed class AutomaticMigrationSettings
{
	public bool Enabled { get; set; }

	public string? Target { get; set; }
}