using DragonSpark.Testing.Objects;

namespace DragonSpark.Testing.Environment.Platform.Development;

public sealed class HelloWorld : IHelloWorld
{
	public static HelloWorld Default { get; } = new();

	HelloWorld() {}

	public string GetMessage() => "Hello From Platform-specific Development!";
}