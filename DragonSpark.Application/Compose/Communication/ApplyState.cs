using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using NetFabric.Hyperlinq;
using System;
using System.Net;
using System.Net.Http;

namespace DragonSpark.Application.Compose.Communication
{
	sealed class ApplyState : ICommand<(IServiceProvider, HttpClient)>
	{
		public static ApplyState Default { get; } = new ApplyState();

		ApplyState() {}

		public void Execute((IServiceProvider, HttpClient) parameter)
		{
			var (provider, client) = parameter;

			var values = provider.GetRequiredService<IHttpContextAccessor>()
			                     .HttpContext.Verify()
			                     .Request.Cookies.AsValueEnumerable()
			                     .Select(x => (string)new StringValues($"{x.Key}={x.Value}"))
			                     .ToArray();

			client.DefaultRequestHeaders.Add(HttpRequestHeader.Cookie.ToString(), values);
		}
	}
}