namespace DragonSpark.Azure.Storage;

public sealed class FileNameProperty : EntryProperty
{
	public static FileNameProperty Default { get; } = new();

	FileNameProperty() : base("fileName") {}
}