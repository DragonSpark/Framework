using Azure.Core;
using DragonSpark.Azure.Data;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace DragonSpark.Redis;

public sealed class ManagedOptions : IResulting<ConfigurationOptions>
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

	public ValueTask<ConfigurationOptions> Get()
		=> _options.ConfigureForAzureWithTokenCredentialAsync(_credential).ToOperation();
}