using Azure.Core;
using DragonSpark.Azure.Data;
using DragonSpark.Model.Operations.Allocated;
using StackExchange.Redis;

namespace DragonSpark.Redis;

public sealed class ManagedOptions : IAllocatedResult<ConfigurationOptions>
{
	readonly ConfigurationOptions _options;
	readonly TokenCredential      _credential;

	public ManagedOptions(DistributedMemoryConnection connection) : this(connection, DefaultCredential.Default) {}

	ManagedOptions(DistributedMemoryConnection connection, TokenCredential credential)
		: this(ConfigurationOptions.Parse(connection.Get().ToString()), credential) {}

	ManagedOptions(ConfigurationOptions options, TokenCredential credential)
	{
		_options    = options;
		_credential = credential;
	}

	public Task<ConfigurationOptions> Get() => _options.ConfigureForAzureWithTokenCredentialAsync(_credential);
}