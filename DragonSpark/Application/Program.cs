using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application;

public class Program : IProgram
{
	readonly Func<IHostBuilder, IHostBuilder> _select;
	readonly IOperation<IHost>                _run;

	protected Program(ISelect<IHostBuilder, IHostBuilder> select) : this(select.Get) {}

	protected Program(Func<IHostBuilder, IHostBuilder> select) : this(@select, RunInitializedProgram.Default) {}

	protected Program(Func<IHostBuilder, IHostBuilder> select, IOperation<IHost> run)
	{
		_select = @select;
		_run    = run;
	}

	public Task Get(Array<string> arguments) => Get(Host.CreateDefaultBuilder(arguments));

	public Task Get(IHostBuilder parameter) => _run.Get(_select(parameter).Build()).AsTask();
}