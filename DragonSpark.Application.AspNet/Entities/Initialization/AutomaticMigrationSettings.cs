namespace DragonSpark.Application.AspNet.Entities.Initialization;

public sealed class AutomaticMigrationSettings
{
	public bool Enabled { get; set; }

	public string? Target { get; set; }
}