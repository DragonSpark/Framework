using DragonSpark.Model.Selection.Stores;
using System;
using System.Collections.Concurrent;
using System.Net.Http;

namespace DragonSpark.Services.Communication
{
	sealed class ClientStore : EqualityStore<Uri, HttpClient>
	{
		public static ClientStore Default { get; } = new ClientStore();

		ClientStore() : base(Clients.Default, new ConcurrentDictionary<Uri, HttpClient>()) {}
	}
}