namespace DragonSpark.Azure.Storage.Uploads;

sealed class SessionValue : FormChunkValue<Guid>
{
	public static SessionValue Default { get; } = new();

	SessionValue() : base("session", Guid.Parse) {}
}