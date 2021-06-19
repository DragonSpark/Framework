using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Model
{
	sealed class MemoryAwareNavigateToSignOut<T> : INavigateToSignOut where T : class
	{
		readonly INavigateToSignOut _previous;
		readonly IMemoryCache       _memory;
		readonly StoredStateKey<T>  _key;

		public MemoryAwareNavigateToSignOut(INavigateToSignOut previous, IMemoryCache memory)
			: this(previous, memory, StoredStateKey<T>.Default) {}

		public MemoryAwareNavigateToSignOut(INavigateToSignOut previous, IMemoryCache memory, StoredStateKey<T> key)
		{
			_previous = previous;
			_memory   = memory;
			_key      = key;
		}

		public void Execute(ClaimsPrincipal parameter)
		{
			_memory.Remove(_key.Get(parameter));
			_previous.Execute(parameter);
		}
	}
}