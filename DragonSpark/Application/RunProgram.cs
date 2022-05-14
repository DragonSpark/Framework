using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace DragonSpark.Application;

sealed class RunProgram : IOperation<IHost>
{
	public static RunProgram Default { get; } = new();

	RunProgram() {}

	public ValueTask Get(IHost parameter) => parameter.RunAsync().ToOperation();
}