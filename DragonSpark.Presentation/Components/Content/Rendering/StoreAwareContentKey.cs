using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class StoreAwareContentKey : StandardTable<ContentKeyInput, string>, IContentKey
{
	public StoreAwareContentKey(IContentKey select) : base(select.Get) {}
}