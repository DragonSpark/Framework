using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace DragonSpark.Application.Entities
{
	sealed class StorageInitializer<T> : IStorageInitializer<T> where T : DbContext
	{
		readonly IMemoryCache _cache;
		readonly Func<T, T> _select;
		readonly object _key;

		public StorageInitializer(IMemoryCache cache, params IInitializer<T>[] initializers)
			: this(cache, initializers.Alter, A.Type<StorageInitializer<T>>()) {}

		StorageInitializer(IMemoryCache cache, Func<T, T> select, object key)
		{
			_cache = cache;
			_select = select;
			_key = key;
		}

		public T Get(T parameter)
		{
			if (!_cache.TryGetValue(_key, out _))
			{
				_cache.Set(_key, this);
				return _select(parameter);
			}
			return parameter;
		}
	}
}