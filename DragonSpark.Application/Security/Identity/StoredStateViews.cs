using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	sealed class StoredStateViews<T> : IStateViews<T> where T : class
	{
		readonly IStateViews<T> _previous;
		readonly IMemoryCache   _store;
		readonly string         _prefix;

		[UsedImplicitly]
		public StoredStateViews(IStateViews<T> previous, IMemoryCache store)
			: this(previous, store, A.Type<StoredStateViews<T>>().AssemblyQualifiedName) {}

		[UsedImplicitly]
		public StoredStateViews(IStateViews<T> previous, IMemoryCache store, string prefix)
		{
			_previous = previous;
			_store    = store;
			_prefix   = prefix;
		}

		public async ValueTask<StateView<T>> Get(ClaimsPrincipal parameter)
		{
			// TODO: Encapsulate:
			var key = $"{_prefix}+{parameter.Identity.Name ?? "Anonymous"}";
			var result = _store.TryGetValue(key, out var stored)
				             ? stored.To<StateView<T>>()
				             : _store.Set(key, await _previous.Get(parameter).ConfigureAwait(false),
				                          TimeSpan.FromSeconds(10));
			return result;
		}
	}
}