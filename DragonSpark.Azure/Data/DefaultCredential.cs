using Azure.Core;
using Azure.Identity;
using DragonSpark.Model.Results;

namespace DragonSpark.Azure.Data;

public sealed class DefaultCredential : Instance<TokenCredential>
{
	public static DefaultCredential Default { get; } = new();

	DefaultCredential() : base(new DefaultAzureCredential()) {}
}