using DragonSpark.Application.Security;
using DragonSpark.Text;
using Microsoft.AspNetCore.Http.Connections.Client;
using System.Net;

namespace DragonSpark.Application.Connections.Client;

sealed class CurrentClientState : IFormatter<HttpConnectionOptions>
{
	readonly ICurrentContext _accessor;
	readonly string          _key;

	public CurrentClientState(ICurrentContext accessor) : this(accessor, HttpRequestHeader.Cookie.ToString()) {}

	public CurrentClientState(ICurrentContext accessor, string key)
	{
		_accessor = accessor;
		_key      = key;
	}

	public string Get(HttpConnectionOptions parameter) => _accessor.Get().Request.Headers[_key]!;
}