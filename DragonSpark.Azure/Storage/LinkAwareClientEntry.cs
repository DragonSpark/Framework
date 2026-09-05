namespace DragonSpark.Azure.Storage;

sealed class LinkAwareClientEntry : ClientEntryBase
{
	public static LinkAwareClientEntry Default { get; } = new();

	LinkAwareClientEntry() : base(LinkAwareLoadStorageEntry.Default) {}
}