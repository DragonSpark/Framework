using DragonSpark.Model.Operations;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application;

public sealed class RunInitializedProgram : Appending<IHost>
{
	public static RunInitializedProgram Default { get; } = new();

	RunInitializedProgram() : base(HostInitialization.Default, RunProgram.Default) {}
}