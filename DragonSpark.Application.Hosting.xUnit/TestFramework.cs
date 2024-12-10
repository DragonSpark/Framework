using System.Reflection;
using JetBrains.Annotations;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DragonSpark.Application.Hosting.xUnit;

[UsedImplicitly, MustDisposeResource]
public sealed class TestFramework : XunitTestFramework
{
	public TestFramework(IMessageSink messageSink) : base(messageSink) {}

	[HandlesResourceDisposal, MustDisposeResource]
	protected override ITestFrameworkExecutor CreateExecutor(AssemblyName assemblyName)
		=> new Executor(assemblyName, SourceInformationProvider, DiagnosticMessageSink);
}
