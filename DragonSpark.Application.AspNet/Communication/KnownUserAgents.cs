using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Communication;

sealed class KnownUserAgents : Instances<string>
{
	public static KnownUserAgents Default { get; } = new();

	KnownUserAgents()
		: base("AlwaysOn", "HostnameSyncPinger", "SiteWarmup", "IIS Application Initialization Warmup",
		       "IIS Application Initialization Preload") {}
}