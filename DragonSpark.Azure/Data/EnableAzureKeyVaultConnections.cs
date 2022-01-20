using Azure.Core;
using DragonSpark.Model.Commands;

namespace DragonSpark.Azure.Data;

public sealed class EnableAzureKeyVaultConnections : DelegatedParameterCommand<TokenCredential>
{
	public static EnableAzureKeyVaultConnections Default { get; } = new();

	EnableAzureKeyVaultConnections() : base(RegisterAzureKeyVaultConnections.Default, DefaultCredential.Default) {}
}