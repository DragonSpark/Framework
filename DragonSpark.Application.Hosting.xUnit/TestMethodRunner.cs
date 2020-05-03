using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DragonSpark.Application.Hosting.xUnit
{
	sealed class TestMethodRunner : XunitTestMethodRunner
	{
		readonly object[]     _constructorArguments;
		readonly IMessageSink _diagnosticMessageSink;

		// ReSharper disable once TooManyDependencies
		public TestMethodRunner(ITestMethod testMethod, IReflectionTypeInfo @class, IReflectionMethodInfo method,
		                        IEnumerable<IXunitTestCase> testCases, IMessageSink diagnosticMessageSink,
		                        IMessageBus messageBus, ExceptionAggregator aggregator,
		                        CancellationTokenSource cancellationTokenSource, object[] constructorArguments)
			: base(testMethod, @class, method, testCases, diagnosticMessageSink, messageBus, aggregator,
			       cancellationTokenSource,
			       constructorArguments)
		{
			_diagnosticMessageSink = diagnosticMessageSink;
			_constructorArguments  = constructorArguments;
		}

		protected override Task<RunSummary> RunTestCaseAsync(IXunitTestCase testCase)
			=> Runner(testCase)?.RunAsync() ?? base.RunTestCaseAsync(testCase);

		XunitTestCaseRunner? Runner(IXunitSerializable testCase)
		{
			switch (testCase)
			{
				case ExecutionErrorTestCase _:
					break;
				case XunitTheoryTestCase @case:
					return new TheoryTestCaseRunner(@case, @case.DisplayName, @case.SkipReason, _constructorArguments,
					                                _diagnosticMessageSink, MessageBus,
					                                new ExceptionAggregator(Aggregator),
					                                CancellationTokenSource);
				case XunitTestCase @case:
					return new TestCaseRunner(@case, @case.DisplayName, @case.SkipReason, _constructorArguments,
					                          @case.TestMethodArguments, MessageBus,
					                          new ExceptionAggregator(Aggregator),
					                          CancellationTokenSource);
			}

			return null;
		}
	}
}