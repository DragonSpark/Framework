namespace DragonSpark.Presentation.Components.Content.History;

sealed class RecentContentTracker : EmptyTracker<ComponentKeyInput, object?>
{
	public RecentContentTracker() : base(_ => string.Empty, 0) {}
}
// TODO
/*sealed class RecentContentTracker : Tracker<ComponentKeyInput, int, object?>
{
	public RecentContentTracker(IComponentKey key) : base(key.Get, HistoryCount.Default) {}
}*/