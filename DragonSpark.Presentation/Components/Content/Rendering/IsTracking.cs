using DragonSpark.Model.Results;

namespace DragonSpark.Presentation.Components.Content.Rendering
{
	sealed class IsTracking : Variable<bool>
	{
		public IsTracking() : base(true) {}
	}
}