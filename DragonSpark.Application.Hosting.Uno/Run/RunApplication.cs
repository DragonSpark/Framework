using DragonSpark.Application.Mobile.Uno.Run;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.Hosting.Uno.Run;

public abstract class RunApplication(
	Func<InitializeInput, IApplicationBuilder> builder,
	Func<IApplicationBuilder, Task<IHost>> host)
	: Mobile.Uno.Run.RunApplication(builder, host)
{
	protected RunApplication(Func<IHostBuilder, IHostBuilder> select, Action<IApplicationBuilder> configure,
	                         Func<IApplicationBuilder, Task<IHost>> host)
		: this(new InitializeBuilder(select, configure).Get, host) {}
}

