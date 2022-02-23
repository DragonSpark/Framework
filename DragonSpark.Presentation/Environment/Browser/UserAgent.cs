using DragonSpark.Model.Selection.Stores;
using DragonSpark.Text;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class UserAgent : ReferenceValueStore<HttpRequest, string>, IFormatter<HttpRequest>
{
	public static UserAgent Default { get; } = new();

	UserAgent() : base(x => x.Headers["User-Agent"].ToString()) {}
}