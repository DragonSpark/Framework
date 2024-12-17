using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using System.Threading.Tasks;

namespace DragonSpark.Application.Mobile.Run;

sealed class EnvironmentalConfigurationAwareRunApplication(IRunApplication previous, ICommand<Application> configure)
	: IRunApplication
{
	readonly IRunApplication       _previous  = previous;
	readonly ICommand<Application> _configure = configure;

	public EnvironmentalConfigurationAwareRunApplication(IRunApplication previous)
		: this(previous, ConfigureFromEnvironment.Default) {}

	public async Task<Application> Get(InitializeInput parameter)
	{
		var result = await _previous.Await(parameter);
		_configure.Execute(result);
		return result;
	}
}