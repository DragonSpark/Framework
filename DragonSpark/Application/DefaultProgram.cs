using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace DragonSpark.Application {
	public sealed class DefaultProgram : IProgram
	{
		public static DefaultProgram Default { get; } = new DefaultProgram();

		DefaultProgram() {}

		public Task Get(IHost parameter) => parameter.RunAsync();
	}
}