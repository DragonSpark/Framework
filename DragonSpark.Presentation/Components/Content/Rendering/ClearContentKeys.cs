using DragonSpark.Model;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Components.Content.Rendering
{
	sealed class ClearContentKeys : ICommand
	{
		readonly IMemoryCache      _memory;
		readonly RenderContentKeys _keys;

		public ClearContentKeys(IMemoryCache memory, RenderContentKeys keys)
		{
			_memory = memory;
			_keys   = keys;
		}

		public void Execute(None parameter)
		{
			if (_keys.Count > 0)
			{
				foreach (var key in _keys)
				{
					_memory.Remove(key);
				}

				_keys.Clear();
			}
		}
	}
}