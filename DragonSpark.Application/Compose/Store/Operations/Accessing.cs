﻿using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Compose.Store.Operations
{
	class Accessing<TIn, TOut> : ISelecting<TIn, TOut>
	{
		readonly Access            _access;
		readonly Get<TIn, TOut>    _get;
		readonly Func<TIn, object> _key;

		public Accessing(Access access, Get<TIn, TOut> get, Func<TIn, object> key)
		{
			_access = access;
			_get    = get;
			_key    = key;
		}

		public async ValueTask<TOut> Get(TIn parameter)
		{
			var key = _key(parameter);
			var result = _access(key, out var stored) && !(stored is null)
				             ? stored.To<TOut>()
				             : await _get((parameter, key));
			return result;
		}
	}
}