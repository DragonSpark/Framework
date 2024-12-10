using System.Collections.Generic;
using System.Reflection;
using DragonSpark.Runtime.Environment;
using DragonSpark.Runtime.Execution;
using JetBrains.Annotations;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DragonSpark.Application.Hosting.xUnit;

[MustDisposeResource]
public class Executor : XunitTestFrameworkExecutor
{
	public Executor(AssemblyName assemblyName,
	                ISourceInformationProvider sourceInformationProvider,
	                IMessageSink diagnosticMessageSink)
		: base(assemblyName, sourceInformationProvider, diagnosticMessageSink) {}

	protected override void RunTestCases(IEnumerable<IXunitTestCase> testCases,
	                                     IMessageSink executionMessageSink,
	                                     ITestFrameworkExecutionOptions executionOptions)
	{
		StorageTypeDefinition.Default.Execute(typeof(Logical<>));
		base.RunTestCases(testCases, executionMessageSink, executionOptions);
	}
}
