using DragonSpark.Application.AspNet;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DragonSpark.Server.Security.Content;

public sealed class ApplyPolicy : ApplyHeader
{
	public ApplyPolicy(RequestDelegate next, ContentSecurityConfiguration settings) : this(next, settings.Policy) {}

	public ApplyPolicy(RequestDelegate next, string value)
		: base(next, "Content-Security-Policy", x => string.Format(value, x.Nonce())) {}

	public Task Invoke(HttpContext parameter) => Get(parameter); // Sigh...
}