namespace DragonSpark.Azure.Storage;

sealed class ClientEntry : ClientEntryBase
{
	public static ClientEntry Default { get; } = new();

	ClientEntry() : base(LoadStorageEntry.Default) {}
}