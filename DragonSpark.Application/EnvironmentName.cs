using DragonSpark.Runtime;

namespace DragonSpark.Application;

public sealed class EnvironmentName : EnvironmentVariable
{
	public static EnvironmentName Default { get; } = new EnvironmentName();

	EnvironmentName() : base("ASPNETCORE_ENVIRONMENT") {}
}