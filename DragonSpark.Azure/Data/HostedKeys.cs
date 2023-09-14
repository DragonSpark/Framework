using Azure.Core;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.DataProtection;

namespace DragonSpark.Azure.Data;

public sealed class HostedKeys : IAlteration<IDataProtectionBuilder>
{
	public static HostedKeys Default { get; } = new();

	HostedKeys() : this(DefaultCredential.Default) {}

	readonly TokenCredential _credential;

	public HostedKeys(TokenCredential credential) => _credential = credential;

	public IDataProtectionBuilder Get(IDataProtectionBuilder parameter)
	{
		var configuration = parameter.Services.Section<HostedKeysConfiguration>().Verify();
		return parameter.ProtectKeysWithAzureKeyVault(new (configuration.Vault), _credential)
		                .PersistKeysToAzureBlobStorage(new (configuration.Location), _credential);
	}
}