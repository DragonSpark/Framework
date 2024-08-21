using DragonSpark.Application.Run;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Application.Hosting.Server.Run;

sealed class SelectBuilder : ISelect<IHostedApplicationBuilder, IHostBuilder>
{
	readonly Func<IHostBuilder, IHostBuilder>    _select;
	readonly ICommand<IHostedApplicationBuilder> _previous;

	public SelectBuilder(Func<IHostBuilder, IHostBuilder> select, ICommand<IHostedApplicationBuilder> previous)
	{
		_select   = select;
		_previous = previous;
	}

	public IHostBuilder Get(IHostedApplicationBuilder parameter)
	{
		var select  = _select(parameter.Builder);
		var builder = parameter.With(select);
		_previous.Execute(builder);
		return builder.Builder;
	}
}