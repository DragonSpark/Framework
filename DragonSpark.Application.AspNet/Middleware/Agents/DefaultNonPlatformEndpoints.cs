using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Middleware.Agents;

public sealed class DefaultNonPlatformEndpoints : Instances<string>
{
	public static DefaultNonPlatformEndpoints Default { get; } = new();

	DefaultNonPlatformEndpoints() : base("/wp-", "/.env", "/.git", "/xmlrpc", "/phpmyadmin") {}
}