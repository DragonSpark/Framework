using DragonSpark.Testing.Objects;

namespace DragonSpark.Testing.Environment;

public sealed class HelloWorld : IHelloWorld
{
	public static HelloWorld Default { get; } = new();

	HelloWorld() {}

	public string GetMessage() => "Hello From Release!";
}