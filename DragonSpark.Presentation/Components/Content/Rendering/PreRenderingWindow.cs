using System;

namespace DragonSpark.Presentation.Components.Content.Rendering
{
	sealed class PreRenderingWindow : DragonSpark.Model.Results.Instance<TimeSpan>
	{
		public static PreRenderingWindow Default { get; } = new();

		PreRenderingWindow() : base(TimeSpan.FromSeconds(1.5)) {}
	}
}