namespace DragonSpark.Azure.Storage.Uploads;

public sealed class TemporaryRoot : Text.Text
{
	public TemporaryRoot(FileStorageSettings settings) : base(settings.TransientRootPath) {}
}