using System;

namespace DragonSpark.Presentation.Components.Content.Rendering
{
	sealed class StoreAwareRenderContentKey : IRenderContentKey
	{
		readonly IRenderContentKey _previous;
		readonly RenderContentKeys _store;

		public StoreAwareRenderContentKey(IRenderContentKey previous, RenderContentKeys store)
		{
			_previous = previous;
			_store    = store;
		}

		public string Get(Delegate parameter)
		{
			var previous = _previous.Get(parameter);
			_store.Add(previous);
			return previous;
		}
	}
}