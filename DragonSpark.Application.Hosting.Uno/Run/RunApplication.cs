using System;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Run;
using Microsoft.Extensions.Hosting;
using Uno.Extensions.Hosting;

namespace DragonSpark.Application.Hosting.Uno.Run;

public abstract class RunApplication(
	Func<InitializeInput, IApplicationBuilder> builder,
	Func<IApplicationBuilder, Task<IHost>> host)
	: Mobile.Run.RunApplication(builder, host)
{
	protected RunApplication(Func<IHostBuilder, IHostBuilder> select, Action<IApplicationBuilder> configure,
	                         Func<IApplicationBuilder, Task<IHost>> host)
		: this(new InitializeBuilder(select, configure).Get, host) {}
}
