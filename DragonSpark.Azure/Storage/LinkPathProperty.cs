namespace DragonSpark.Azure.Storage;

public sealed class LinkPathProperty : EntryProperty
{
	public static LinkPathProperty Default { get; } = new();

	LinkPathProperty() : base("linkPath") {}
}