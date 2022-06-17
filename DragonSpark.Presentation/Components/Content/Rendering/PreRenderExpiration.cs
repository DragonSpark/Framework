using DragonSpark.Application.Model;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class PreRenderExpiration : SlidingExpiration
{
	public static PreRenderExpiration Default { get; } = new();

	PreRenderExpiration() : base(PreRenderingWindow.Default) {}
}