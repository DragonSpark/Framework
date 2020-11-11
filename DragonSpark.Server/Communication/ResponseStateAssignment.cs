using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Net.Http;

namespace DragonSpark.Server.Communication
{
	class ResponseStateAssignment : IAssign<HttpRequest, HttpClient>
	{
		readonly static Func<HttpClient, System.Net.Http.HttpClientHandler> Handler = AssociatedHandlers.Default.Get;

		readonly Func<HttpClient, System.Net.Http.HttpClientHandler> _handler;
		readonly Func<HttpRequest, Cookie>                           _state;

		public ResponseStateAssignment(Func<HttpRequest, Cookie> state) : this(Handler, state) {}

		public ResponseStateAssignment(Func<HttpClient, System.Net.Http.HttpClientHandler> handler,
		                               Func<HttpRequest, Cookie> state)
		{
			_handler = handler;
			_state   = state;
		}

		public void Execute(Pair<HttpRequest, HttpClient> parameter)
		{
			_handler(parameter.Value)
				.CookieContainer.Add(parameter.Value.BaseAddress.Verify(),
				                     _state(parameter.Key) ?? throw new InvalidOperationException());
		}
	}
}