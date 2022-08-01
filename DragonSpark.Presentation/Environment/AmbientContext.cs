using DragonSpark.Runtime.Execution;
using Microsoft.AspNetCore.SignalR;

namespace DragonSpark.Presentation.Environment;

sealed class AmbientContext : Logical<HubCallerContext>
{
	public static AmbientContext Default { get; } = new();

	AmbientContext() {}
}