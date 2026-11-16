namespace DragonSpark.Presentation.Components.Content.History;

sealed class RecentContentTracker : Tracker<ComponentKeyInput, int, object?>
{
	public RecentContentTracker(IComponentKey key) : base(key.Get, HistoryCount.Default) {}
}