using DragonSpark.Model.Operations;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application
{
	sealed class RunInitializedProgram : Appending<IHost>
	{
		public static RunInitializedProgram Default { get; } = new RunInitializedProgram();

		RunInitializedProgram() : base(HostInitialization.Default, RunProgram.Default) {}
	}
}