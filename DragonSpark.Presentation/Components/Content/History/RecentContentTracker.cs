using DragonSpark.Presentation.Components.Content.Rendering;

namespace DragonSpark.Presentation.Components.Content.History;

sealed class RecentContentTracker : Tracker<Microsoft.AspNetCore.Components.ComponentBase, object?>
{
	public RecentContentTracker(IRenderContentKey key) : base(key.Get, HistoryCount.Default) {}
}