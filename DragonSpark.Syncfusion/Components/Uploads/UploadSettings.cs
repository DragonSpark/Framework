namespace DragonSpark.SyncfusionRendering.Components.Uploads;

public class UploadSettings
{
	public required string Location { get; init; }

	public required string Save { get; init; }

	public required string Remove { get; init; }

	public uint Size { get; set; } = 1_048_576; // 1MB
}