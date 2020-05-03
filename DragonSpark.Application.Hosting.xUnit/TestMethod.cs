using Xunit.Abstractions;

namespace DragonSpark.Application.Hosting.xUnit
{
	sealed class TestMethod : ITestMethod
	{
		readonly ITestMethod _method = default!;

		public TestMethod() {}

		public TestMethod(ITestMethod method) : this(method, new Decorated(method.Method)) {}

		public TestMethod(ITestMethod method, IMethodInfo info)
		{
			_method = method;
			Method  = info;
		}

		public void Deserialize(IXunitSerializationInfo info)
		{
			_method.Deserialize(info);
		}

		public void Serialize(IXunitSerializationInfo info)
		{
			_method.Serialize(info);
		}

		public IMethodInfo Method { get; } = default!;

		public ITestClass TestClass => _method.TestClass;
	}
}