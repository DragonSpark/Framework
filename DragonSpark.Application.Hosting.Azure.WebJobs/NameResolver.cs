using DragonSpark.Azure.Messaging.Messages;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

sealed class NameResolver : INameResolver
{
	readonly string                _audience;
	readonly bool                  _apply;
	readonly string                _name;
	readonly ILogger<NameResolver> _logger;

	public NameResolver(ServiceBusConfiguration configuration, ILogger<NameResolver> logger)
		: this(configuration.Audience, logger) {}

	public NameResolver(string? audience, ILogger<NameResolver> logger)
		: this(audience ?? string.Empty, !string.IsNullOrEmpty(audience),
		       nameof(ServiceBusConfiguration.Audience).ToLowerInvariant(), logger) {}

	public NameResolver(string audience, bool apply, string name, ILogger<NameResolver> logger)
	{
		_audience    = audience;
		_apply       = apply;
		_name        = name;
		_logger = logger;
	}

	public string Resolve(string name)
	{
		var audience = _apply && _name == name ? _audience : string.Empty;
		_logger.LogInformation("Resolve: {Name} {Audience} {Result}", name, _name, audience);
		return audience;
	}
}