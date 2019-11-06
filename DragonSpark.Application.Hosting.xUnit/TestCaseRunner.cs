using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DragonSpark.Application.Hosting.xUnit
{
	sealed class TestCaseRunner : XunitTestCaseRunner
	{
		// ReSharper disable once TooManyDependencies
		public TestCaseRunner(IXunitTestCase testCase, string displayName, string skipReason,
		                      object[] constructorArguments,
		                      object[] testMethodArguments, IMessageBus messageBus, ExceptionAggregator aggregator,
		                      CancellationTokenSource cancellationTokenSource)
			: base(testCase, displayName, skipReason, constructorArguments, testMethodArguments, messageBus, aggregator,
			       cancellationTokenSource) {}

		// ReSharper disable once TooManyArguments
		protected override XunitTestRunner CreateTestRunner(ITest test, IMessageBus messageBus, Type testClass,
		                                                    object[] constructorArguments,
		                                                    MethodInfo testMethod, object[] testMethodArguments,
		                                                    string skipReason,
		                                                    IReadOnlyList<BeforeAfterTestAttribute>
			                                                    beforeAfterAttributes,
		                                                    ExceptionAggregator aggregator,
		                                                    CancellationTokenSource cancellationTokenSource)
			=> new TestRunner(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments,
			                  skipReason, beforeAfterAttributes, new ExceptionAggregator(aggregator),
			                  cancellationTokenSource);
	}
}