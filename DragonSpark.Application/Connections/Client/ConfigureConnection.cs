using DragonSpark.Application.Security;
using Microsoft.AspNetCore.Http.Connections.Client;
using System.Net;

namespace DragonSpark.Application.Connections.Client;

sealed class ConfigureConnection : IConfigureConnection
{
	readonly ICurrentContext _accessor;
	readonly string          _key;

	public ConfigureConnection(ICurrentContext accessor) : this(accessor, HttpRequestHeader.Cookie.ToString()) {}

	public ConfigureConnection(ICurrentContext accessor, string key)
	{
		_accessor = accessor;
		_key      = key;
	}

	public void Execute(HttpConnectionOptions parameter)
	{
		parameter.Headers[_key] = _accessor.Get().Request.Headers[_key];
	}
}