using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using Microsoft.Extensions.Hosting;
using Uno.Extensions.Hosting;

namespace DragonSpark.Application.Mobile.Uno.Run;

public abstract class RunApplication(
	Func<InitializeInput, IApplicationBuilder> builder,
	Func<IApplicationBuilder, Task<IHost>> host)
	: IRunApplication
{
	readonly Func<InitializeInput, IApplicationBuilder> _builder = builder;
	readonly Func<IApplicationBuilder, Task<IHost>>     _host    = host;

	public async Task<Application> Get(InitializeInput parameter)
	{
		var builder = _builder(parameter);
		return new(builder, await _host(builder).Off());
	}
}
