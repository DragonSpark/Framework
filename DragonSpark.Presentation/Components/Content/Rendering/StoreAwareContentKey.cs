using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class StoreAwareContentKey : StandardTable<object, string>, IContentKey // TODO: This is bad, very bad
{
	public StoreAwareContentKey(IContentKey select) : base(select.Get) {}
}