using DragonSpark.Model.Operations.Allocated;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace DragonSpark.Application;

sealed class RunProgram : IAllocated<IHost>
{
	public static RunProgram Default { get; } = new();

	RunProgram() {}

	public Task Get(IHost parameter) => parameter.RunAsync();
}