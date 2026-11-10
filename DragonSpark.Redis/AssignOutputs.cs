using Microsoft.AspNetCore.OutputCaching.StackExchangeRedis;

namespace DragonSpark.Redis;

sealed class AssignOutputs : AssignInstanceBase<RedisOutputCacheOptions>
{
	public static AssignOutputs Default { get; } = new();

	AssignOutputs() : base(x => x.InstanceName, (x, v) => x.InstanceName = v) {}
}