using Azure.Core;
using DragonSpark.Composition;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.DataProtection;
using System;

namespace DragonSpark.Azure.Data;

public sealed class HostedKeys : IAlteration<IDataProtectionBuilder>
{
	public static HostedKeys Default { get; } = new();

	HostedKeys() : this(DefaultCredential.Default) {}

	readonly TokenCredential _credential;

	public HostedKeys(TokenCredential credential) => _credential = credential;

	public IDataProtectionBuilder Get(IDataProtectionBuilder parameter)
	{
		var configuration = parameter.Services.Section<HostedKeysConfiguration>();
		return parameter.ProtectKeysWithAzureKeyVault(new Uri(configuration.Vault), _credential)
		                .PersistKeysToAzureBlobStorage(new Uri(configuration.Location), _credential);
	}
}