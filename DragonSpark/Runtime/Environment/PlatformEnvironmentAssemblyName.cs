namespace DragonSpark.Runtime.Environment;

sealed class PlatformEnvironmentAssemblyName : ExternalAssemblyName
{
    public PlatformEnvironmentAssemblyName(string platform) : base($"{{0}}.Environment.{platform}") {}
}