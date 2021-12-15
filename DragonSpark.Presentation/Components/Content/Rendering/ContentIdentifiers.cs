using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ContentIdentifiers : ReferenceValueStore<object, ContentIdentifier>
{
	public static ContentIdentifiers Default { get; } = new();

	ContentIdentifiers() : base(_ => new ContentIdentifier()) {}
}