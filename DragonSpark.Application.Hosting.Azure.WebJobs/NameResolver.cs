using DragonSpark.Azure.Messaging.Messages;
using Microsoft.Azure.WebJobs;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

sealed class NameResolver : INameResolver
{
	readonly string _audience;
	readonly bool   _apply;
	readonly string _name;

	public NameResolver(ServiceBusConfiguration configuration) : this(configuration.Audience) {}

	public NameResolver(string? audience)
		: this(audience ?? string.Empty, !string.IsNullOrEmpty(audience),
		       nameof(ServiceBusConfiguration.Audience).ToLowerInvariant()) {}

	public NameResolver(string audience, bool apply, string name)
	{
		_audience = audience;
		_apply    = apply;
		_name     = name;
	}

	public string Resolve(string name) => _apply && _name == name ? _audience : string.Empty;
}