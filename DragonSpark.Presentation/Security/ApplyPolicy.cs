using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Security
{
	public sealed class ApplyPolicy : ApplyHeader
	{
		public ApplyPolicy(RequestDelegate next, ContentSecurityConfiguration configuration)
			: this(next, configuration.Policy) {}

		public ApplyPolicy(RequestDelegate next, string value) : base(next, "Content-Security-Policy", value) {}

		public Task Invoke(HttpContext parameter) => Get(parameter); // Sigh...
	}
}