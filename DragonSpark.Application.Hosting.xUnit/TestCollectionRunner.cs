using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DragonSpark.Application.Hosting.xUnit
{
	public class TestCollectionRunner : XunitTestCollectionRunner
	{
		readonly IMessageSink diagnosticMessageSink;

		// ReSharper disable once TooManyDependencies
		public TestCollectionRunner(ITestCollection testCollection,
		                            IEnumerable<IXunitTestCase> testCases,
		                            IMessageSink diagnosticMessageSink,
		                            IMessageBus messageBus,
		                            ITestCaseOrderer testCaseOrderer,
		                            ExceptionAggregator aggregator,
		                            CancellationTokenSource cancellationTokenSource)
			: base(testCollection, testCases, diagnosticMessageSink, messageBus, testCaseOrderer, aggregator,
			       cancellationTokenSource) => this.diagnosticMessageSink = diagnosticMessageSink;

		protected override Task<RunSummary> RunTestClassAsync(ITestClass testClass, IReflectionTypeInfo @class,
		                                                      IEnumerable<IXunitTestCase> testCases)
			=> new TestClassRunner(testClass, @class, testCases, diagnosticMessageSink, MessageBus, TestCaseOrderer,
			                       new ExceptionAggregator(Aggregator), CancellationTokenSource,
			                       CollectionFixtureMappings)
				.RunAsync();
	}
}