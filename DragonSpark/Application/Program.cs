using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application;

public class Program : IProgram
{
	readonly Func<string[], IHostBuilder>     _create;
	readonly Func<IHostBuilder, IHostBuilder> _select;
	readonly IAllocated<IHost>                _run;

	protected Program(ISelect<IHostBuilder, IHostBuilder> select) : this(select.Get) {}

	protected Program(Func<IHostBuilder, IHostBuilder> select) : this(Host.CreateDefaultBuilder, select) {}

	protected Program(Func<string[], IHostBuilder> create, Func<IHostBuilder, IHostBuilder> select)
		: this(create, select, RunInitializedProgram.Default) {}

	protected Program(Func<string[], IHostBuilder> create, Func<IHostBuilder, IHostBuilder> select,
	                  IAllocated<IHost> run)
	{
		_create = create;
		_select = select;
		_run    = run;
	}

	public Task Get(Array<string> arguments) => Get(_create(arguments));

	public Task Get(IHostBuilder parameter) => _run.Get(_select(parameter).Build());

	protected IHostBuilder Create(string[] parameter) => _select(_create(parameter));
}