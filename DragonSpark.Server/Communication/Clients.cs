using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Net.Http;

namespace DragonSpark.Server.Communication
{
	sealed class Clients : ISelect<Uri, HttpClient>
	{
		public static Clients Default { get; } = new Clients();

		Clients() : this(AssociatedHandlers.Default, ClientHandlers.Default.Get) {}

		readonly ITable<HttpClient, System.Net.Http.HttpClientHandler> _associated;
		readonly Func<Uri, System.Net.Http.HttpClientHandler>          _handlers;

		public Clients(ITable<HttpClient, System.Net.Http.HttpClientHandler> associated,
		               Func<Uri, System.Net.Http.HttpClientHandler> handlers)
		{
			_associated = associated;
			_handlers   = handlers;
		}

		public HttpClient Get(Uri parameter)
		{
			var handler = _handlers(parameter);
			var result  = new HttpClient(handler) {BaseAddress = parameter};
			_associated.Assign(result, handler);
			return result;
		}
	}
}