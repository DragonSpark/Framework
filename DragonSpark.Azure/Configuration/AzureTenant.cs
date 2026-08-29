using DragonSpark.Application.Configuration;

namespace DragonSpark.Azure.Configuration;

sealed class AzureTenant : Assign
{
	public static AzureTenant Default { get; } = new();

	AzureTenant() : base("AZURE_TENANT_ID") {}
}