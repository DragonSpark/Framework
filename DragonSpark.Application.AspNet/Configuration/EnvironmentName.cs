using DragonSpark.Runtime;

namespace DragonSpark.Application.AspNet.Configuration;

public sealed class EnvironmentName : EnvironmentVariable
{
	public static EnvironmentName Default { get; } = new();

	EnvironmentName() : base("ASPNETCORE_ENVIRONMENT") {}
}