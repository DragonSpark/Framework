using System.Threading.Tasks;
using DragonSpark.Compose;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Run;

public sealed class RunInitializedApplication : IRunApplication
{
	readonly IRunApplication _previous;

	public RunInitializedApplication(IRunApplication previous) => _previous = previous;

	public async Task<Application> Get(InitializeInput parameter)
	{
		var result  = await _previous.Off(parameter);
		var service = result.Host.Services.GetService<IInitializeApplication>();
		if (service is not null)
		{
			await service.Off(result);
		}

		return result;
	}
}