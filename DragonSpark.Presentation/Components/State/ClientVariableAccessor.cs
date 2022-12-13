using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Http;
using System;

namespace DragonSpark.Presentation.Components.State;

public class ClientVariableAccessor<T> : ISelect<HttpContext, T?>
{
	readonly string          _key;
	readonly Func<string, T> _parse;

	protected ClientVariableAccessor(string key, Func<string, T> parse)
	{
		_key   = key;
		_parse = parse;
	}

	public T? Get(HttpContext parameter)
	{
		var store  = parameter.Request.Cookies;
		// ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
		var result = store.TryGetValue(_key, out var accessed) && accessed is not null ? _parse(accessed) : default; // ISSUE
		return result;
	}
}