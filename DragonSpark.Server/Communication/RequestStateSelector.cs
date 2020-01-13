using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Server.Communication
{
	sealed class RequestStateSelector : Select<HttpRequest, IRequestCookieCollection>
	{
		public static RequestStateSelector Default { get; } = new RequestStateSelector();

		RequestStateSelector() : base(x => x.Cookies) {}
	}
}