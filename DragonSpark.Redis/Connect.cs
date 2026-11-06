using Azure.Core;
using DragonSpark.Azure.Data;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Allocated;
using StackExchange.Redis;

namespace DragonSpark.Redis;

sealed class Connect : IAllocatedResult<IConnectionMultiplexer>
{
	readonly ConfigurationOptions _options;
	readonly TokenCredential      _credential;

	public Connect(DistributedMemoryConnection connection) : this(connection, DefaultCredential.Default) {}

	Connect(DistributedMemoryConnection connection, TokenCredential credential)
		: this(ConfigurationOptions.Parse(connection.Get().ToString()), credential) {}

	Connect(ConfigurationOptions options, TokenCredential credential)
	{
		_options    = options;
		_credential = credential;
	}

	public async Task<IConnectionMultiplexer> Get()
	{
		var options       = _options.Clone();
		var configuration = await options.ConfigureForAzureWithSystemAssignedManagedIdentityAsync().Off();
		return await ConnectionMultiplexer.ConnectAsync(configuration).Off();
	}}