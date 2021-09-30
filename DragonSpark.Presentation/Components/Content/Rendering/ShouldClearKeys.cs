using DragonSpark.Runtime.Execution;

namespace DragonSpark.Presentation.Components.Content.Rendering
{
	sealed class ShouldClearKeys : FirstAssigned
	{
		public ShouldClearKeys(IsTracking store) : base(store) {}
	}
}