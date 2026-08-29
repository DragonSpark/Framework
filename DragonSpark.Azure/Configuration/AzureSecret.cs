using DragonSpark.Application.Configuration;

namespace DragonSpark.Azure.Configuration;

sealed class AzureSecret : Assign
{
	public static AzureSecret Default { get; } = new();

	AzureSecret() : base("AZURE_CLIENT_SECRET") {}
}