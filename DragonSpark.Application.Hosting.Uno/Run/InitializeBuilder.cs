using System;
using DragonSpark.Application.Mobile.Run;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Hosting;
using Uno.Extensions.Hosting;

namespace DragonSpark.Application.Hosting.Uno.Run;

sealed class InitializeBuilder(Func<IHostBuilder, IHostBuilder> host, Action<IApplicationBuilder> configure)
	: ISelect<InitializeInput, IApplicationBuilder>
{
	public IApplicationBuilder Get(InitializeInput parameter)
	{
		var (owner, arguments) = parameter;
		var result = owner.CreateBuilder(arguments).Configure(x => host(x));
		configure(result);
		return result;
	}
}
