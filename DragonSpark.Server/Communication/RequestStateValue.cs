using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Server.Communication
{
	sealed class RequestStateValue : ISelect<IRequestCookieCollection, string?>
	{
		readonly string _name;

		public RequestStateValue(string name) => _name = name;

		public string? Get(IRequestCookieCollection parameter)
			=> parameter.TryGetValue(_name, out var result) ? result : null;
	}
}