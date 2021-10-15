namespace DragonSpark.Runtime.Environment;

sealed class EnvironmentAssemblyName : ExternalAssemblyName
{
	public static EnvironmentAssemblyName Default { get; } = new EnvironmentAssemblyName();

	EnvironmentAssemblyName() : base("{0}.Environment") {}
}