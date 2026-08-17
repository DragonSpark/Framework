namespace DragonSpark.Azure.Storage.Uploads;

public sealed class FileStorageSettings
{
	public string TransientRootPath { get; set; } = ".temporary";
}