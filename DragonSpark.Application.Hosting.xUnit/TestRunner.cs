using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DragonSpark.Application.Hosting.xUnit
{
	sealed class TestRunner : XunitTestRunner
	{
		// ReSharper disable once TooManyDependencies
		public TestRunner(ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments,
		                  MethodInfo testMethod, object[] testMethodArguments, string skipReason,
		                  IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes, ExceptionAggregator aggregator,
		                  CancellationTokenSource cancellationTokenSource)
			: base(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments, skipReason,
			       beforeAfterAttributes, aggregator, cancellationTokenSource) {}

		protected override Task<decimal> InvokeTestMethodAsync(ExceptionAggregator aggregator)
			=> new TestInvoker(Test, MessageBus, TestClass, ConstructorArguments, TestMethod, TestMethodArguments,
			                   BeforeAfterAttributes, aggregator, CancellationTokenSource).RunAsync();
	}
}