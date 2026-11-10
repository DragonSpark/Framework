using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace DragonSpark.Redis;

sealed class AssignMemory : AssignInstanceBase<RedisCacheOptions>
{
	public static AssignMemory Default { get; } = new();

	AssignMemory() : base(x => x.InstanceName, (x, v) => x.InstanceName = v) {}
}