using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class StoreAwareRenderContentKey : StandardTable<object, string>, IRenderContentKey
{
	public StoreAwareRenderContentKey(IRenderContentKey select) : base(select.Get) {}
}