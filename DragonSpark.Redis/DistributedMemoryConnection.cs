using DragonSpark.Application.Communication;
using Microsoft.Extensions.Configuration;

namespace DragonSpark.Redis;

public sealed class DistributedMemoryConnection : ConnectionPath
{
	public DistributedMemoryConnection(IConfiguration configuration)
		: base(configuration, nameof(DistributedMemoryConnection)) {}
}