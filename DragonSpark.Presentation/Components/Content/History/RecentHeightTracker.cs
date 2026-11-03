namespace DragonSpark.Presentation.Components.Content.History;

sealed class RecentHeightTracker : Tracker<string, double?>
{
	public RecentHeightTracker() : base(s => s, 16) {}
}