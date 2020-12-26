using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace DragonSpark.Application.Compose.Communication
{
	sealed class Configure<T> : ICommand<(IServiceProvider, HttpClient)> where T : notnull
	{
		readonly Func<T, Uri> _baseUri;

		public Configure(Func<T, Uri> baseUri) => _baseUri = baseUri;

		public void Execute((IServiceProvider, HttpClient) parameter)
		{
			var (provider, client) = parameter;
			client.BaseAddress     = _baseUri(provider.GetRequiredService<T>());
		}
	}
}