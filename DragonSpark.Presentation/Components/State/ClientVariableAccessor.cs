using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Http;
using System;

namespace DragonSpark.Presentation.Components.State;

public class ClientVariableAccessor<T> : IResult<T?>
{
	readonly IHttpContextAccessor _accessor;
	readonly string               _key;
	readonly Func<string, T>      _parse;

	protected ClientVariableAccessor(IHttpContextAccessor accessor, string key, Func<string, T> parse)
	{
		_accessor = accessor;
		_key      = key;
		_parse    = parse;
	}

	public T? Get()
	{
		var store  = _accessor.HttpContext.Verify().Request.Cookies;
		var result = store.TryGetValue(_key, out var accessed) && accessed is not null ? _parse(accessed) : default;
		return result;
	}
}