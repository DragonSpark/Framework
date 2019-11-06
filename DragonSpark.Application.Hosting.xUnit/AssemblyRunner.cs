using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DragonSpark.Application.Hosting.xUnit
{
	public class AssemblyRunner : XunitTestAssemblyRunner
	{
		// ReSharper disable once TooManyDependencies
		public AssemblyRunner(ITestAssembly testAssembly,
		                      IEnumerable<IXunitTestCase> testCases,
		                      IMessageSink diagnosticMessageSink,
		                      IMessageSink executionMessageSink,
		                      ITestFrameworkExecutionOptions executionOptions)
			: base(testAssembly, testCases, diagnosticMessageSink, executionMessageSink, executionOptions) {}

		// ReSharper disable once TooManyArguments
		protected override Task<RunSummary> RunTestCollectionAsync(IMessageBus messageBus,
		                                                           ITestCollection testCollection,
		                                                           IEnumerable<IXunitTestCase> testCases,
		                                                           CancellationTokenSource cancellationTokenSource)
			=> new TestCollectionRunner(testCollection, testCases, DiagnosticMessageSink, messageBus, TestCaseOrderer,
			                            new ExceptionAggregator(Aggregator), cancellationTokenSource).RunAsync();
	}
}