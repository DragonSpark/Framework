using DragonSpark.Compose;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace DragonSpark.Application.Mobile.Run;

public sealed class RunInitializedApplication : IRunApplication
{
	readonly IRunApplication _previous;

	public RunInitializedApplication(IRunApplication previous) => _previous = previous;

	public async Task<Application> Get(InitializeInput parameter)
	{
		var result  = await _previous.Await(parameter);
		var service = result.Host.Services.GetService<IInitializeApplication>();
		if (service is not null)
		{
			await service.Await(result);
		}

		return result;
	}
}