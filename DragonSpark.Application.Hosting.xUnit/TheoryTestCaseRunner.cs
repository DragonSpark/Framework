using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DragonSpark.Application.Hosting.xUnit
{
	sealed class TheoryTestCaseRunner : XunitTheoryTestCaseRunner
	{
		// ReSharper disable once TooManyDependencies
		public TheoryTestCaseRunner(IXunitTestCase testCase, string displayName, string skipReason,
		                            object[] constructorArguments, IMessageSink diagnosticMessageSink,
		                            IMessageBus messageBus,
		                            ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource) :
			base(testCase, displayName, skipReason, constructorArguments, diagnosticMessageSink, messageBus, aggregator,
			     cancellationTokenSource) {}

		protected override async Task AfterTestCaseStartingAsync()
		{
			var @case = TestCase;
			TestCase = new TestCase(@case, () => TestCase = @case);
			await base.AfterTestCaseStartingAsync();
		}

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