using DragonSpark.Model.Commands;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ContentInteraction : ValidatedCommand, IContentInteraction
{
	public ContentInteraction(ShouldClearKeys condition, ClearContentKeys command) : base(condition, command) {}
}