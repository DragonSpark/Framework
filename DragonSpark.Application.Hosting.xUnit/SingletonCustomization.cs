using AutoFixture.Kernel;

namespace DragonSpark.Application.Hosting.xUnit;

sealed class SingletonCustomization : InsertCustomization
{
	public static SingletonCustomization Default { get; } = new();

	SingletonCustomization() : base(new MethodInvoker(SingletonQuery.Default)) {}
}