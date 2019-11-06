using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DragonSpark.Application.Hosting.xUnit
{
	sealed class TestClassRunner : XunitTestClassRunner
	{
		// ReSharper disable once TooManyDependencies
		public TestClassRunner(ITestClass testClass, IReflectionTypeInfo @class, IEnumerable<IXunitTestCase> testCases,
		                       IMessageSink diagnosticMessageSink, IMessageBus messageBus,
		                       ITestCaseOrderer testCaseOrderer,
		                       ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource,
		                       IDictionary<Type, object> collectionFixtureMappings)
			: base(testClass, @class, testCases, diagnosticMessageSink, messageBus, testCaseOrderer, aggregator,
			       cancellationTokenSource, collectionFixtureMappings) {}

		// ReSharper disable once TooManyArguments
		protected override Task<RunSummary> RunTestMethodAsync(ITestMethod testMethod, IReflectionMethodInfo method,
		                                                       IEnumerable<IXunitTestCase> testCases,
		                                                       object[] constructorArguments)
			=> new TestMethodRunner(testMethod, Class, method, testCases, DiagnosticMessageSink, MessageBus,
			                        new ExceptionAggregator(Aggregator), CancellationTokenSource, constructorArguments)
				.RunAsync();
	}
}