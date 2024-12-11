using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Allocated;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Uno.Extensions.Hosting;

namespace DragonSpark.Application.Mobile.Run;

public abstract class RunApplication : IAllocating<InitializeInput, Application>
{
	readonly Func<InitializeInput, IApplicationBuilder> _builder;
	readonly Func<IApplicationBuilder, Task<IHost>>     _host;

	protected RunApplication(Func<InitializeInput, IApplicationBuilder> builder,
	                         Func<IApplicationBuilder, Task<IHost>> host)
	{
		_host    = host;
		_builder = builder;
	}

	public async Task<Application> Get(InitializeInput parameter)
	{
		var builder = _builder(parameter);
		return new (builder, await _host(builder).Await());
	}
}

public sealed record Application(IApplicationBuilder Builder, IHost Host);

// TODO
public readonly record struct InitializeInput(Microsoft.UI.Xaml.Application Owner, LaunchActivatedEventArgs Arguments);
