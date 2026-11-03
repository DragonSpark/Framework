namespace DragonSpark.Presentation.Components.Content.History;

sealed class HistoryCount : DragonSpark.Model.Results.Instance<byte>
{
	public static HistoryCount Default { get; } = new();

	HistoryCount() : base(32) {}
}