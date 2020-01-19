using JetBrains.Annotations;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DragonSpark.Application.Hosting.xUnit
{
	[UsedImplicitly]
	public sealed class TestFramework : XunitTestFramework
	{
		public TestFramework(IMessageSink messageSink) : base(messageSink) {}

		protected override ITestFrameworkExecutor CreateExecutor(AssemblyName assemblyName)
			=> new Executor(assemblyName, SourceInformationProvider, DiagnosticMessageSink);
	}
}