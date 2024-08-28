using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.Run;

sealed class ApplicationConfigurationAdapter : ICommand<IApplicationBuilder>
{
	readonly ICommand<IHostApplicationBuilder> _previous;

	public ApplicationConfigurationAdapter(ICommand<IHostApplicationBuilder> previous) => _previous = previous;

	public void Execute(IApplicationBuilder parameter)
	{
		_previous.Execute(ExtensionMethods.To<IHostApplicationBuilder>(parameter));
	}
}