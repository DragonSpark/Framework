using DragonSpark.Compose;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetFabric.Hyperlinq;
using System.Threading.Tasks;

namespace DragonSpark.Application
{
	sealed class HostInitialization : IHostInitializer
	{
		public static HostInitialization Default { get; } = new HostInitialization();

		HostInitialization() {}

		public async ValueTask Get(IHost parameter)
		{
			foreach (var initializer in parameter.Services.GetServices<IHostInitializer>().AsValueEnumerable())
			{
				await initializer.Await(parameter);
			}
		}
	}
}