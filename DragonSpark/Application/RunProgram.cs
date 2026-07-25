using DragonSpark.Model.Operations.Allocated;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application;

sealed class RunProgram : IAllocated<IHost>
{
	public static RunProgram Default { get; } = new();

	RunProgram() {}

	public Task Get(IHost parameter) => parameter.RunAsync();
}