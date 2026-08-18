namespace DragonSpark.Azure.Storage.Uploads;

sealed class WorkspaceValue : FormChunkValue<Guid>
{
	public static WorkspaceValue Default { get; } = new();

	WorkspaceValue() : base("workspace", Guid.Parse) {}
}