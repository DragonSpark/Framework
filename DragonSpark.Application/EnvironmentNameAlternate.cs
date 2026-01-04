using DragonSpark.Runtime;

namespace DragonSpark.Application;

public sealed class EnvironmentNameAlternate : EnvironmentVariable
{
	public static EnvironmentNameAlternate Default { get; } = new();

	EnvironmentNameAlternate() : base("DOTNET_ENVIRONMENT") {}
}