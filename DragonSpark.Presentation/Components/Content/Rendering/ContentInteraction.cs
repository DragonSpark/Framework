using DragonSpark.Model;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Components.Content.Rendering
{
	sealed class ContentInteraction : IContentInteraction
	{
		readonly IsTracking        _switch;
		readonly IMemoryCache      _memory;
		readonly RenderContentKeys _keys;

		public ContentInteraction(IsTracking @switch, IMemoryCache memory, RenderContentKeys keys)
		{
			_switch = @switch;
			_memory = memory;
			_keys   = keys;
		}

		public void Execute(None parameter)
		{
			if (_switch.Get())
			{
				_switch.Execute(false);

				foreach (var key in _keys)
				{
					_memory.Remove(key);
				}
			}

			_keys.Clear();
		}
	}
}