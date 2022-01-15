using DragonSpark.Diagnostics.Logging;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class LogAuthenticationMessage : LogMessage<string, string[]>
{
	public LogAuthenticationMessage(ILogger<LogAuthenticationMessage> logger)
		: base(logger, "{UserName} logged in with claims of {Claims}") {}
}