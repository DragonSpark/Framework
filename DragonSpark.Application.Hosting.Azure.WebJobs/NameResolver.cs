using DragonSpark.Azure.Messaging.Messages;
using Microsoft.Azure.WebJobs;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

sealed class NameResolver : INameResolver
{
	readonly string? _audience;
	readonly bool    _apply;

	public NameResolver(ServiceBusConfiguration configuration) : this(configuration.Audience) {}

	public NameResolver(string? audience) : this(audience, !string.IsNullOrEmpty(audience)) {}

	public NameResolver(string? audience, bool apply)
	{
		_audience = audience;
		_apply    = apply;
	}

	public string Resolve(string name) => _apply ? $"{name}{_audience}" : name;
}