using DragonSpark.Application.Connections;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections.Client;
using System.Net;

namespace DragonSpark.Presentation.Connections
{
	sealed class ConfigureConnection : IConfigureConnection
	{
		readonly IHttpContextAccessor _accessor;
		readonly string               _key;

		public ConfigureConnection(IHttpContextAccessor accessor)
			: this(accessor, HttpRequestHeader.Cookie.ToString()) {}

		public ConfigureConnection(IHttpContextAccessor accessor, string key)
		{
			_accessor = accessor;
			_key      = key;
		}

		public void Execute(HttpConnectionOptions parameter)
		{
			parameter.Headers[_key] = _accessor.HttpContext.Verify().Request.Headers[_key];
		}
	}
}