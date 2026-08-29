using DragonSpark.Application.Configuration;

namespace DragonSpark.Azure.Configuration;

sealed class AzureClient : Assign
{
	public static AzureClient Default { get; } = new();

	AzureClient() : base("AZURE_CLIENT_ID") {}
}