using System;
using System.Threading.Tasks;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application;

public class Program : ProgramBase
{
	readonly Func<string[], IHostBuilder>     _create;
	readonly Func<IHostBuilder, IHostBuilder> _select;

	protected Program(ISelect<IHostBuilder, IHostBuilder> select) : this(select.Get) {}

	protected Program(Func<IHostBuilder, IHostBuilder> select) : this(Host.CreateDefaultBuilder, select) {}

	protected Program(Func<string[], IHostBuilder> create, Func<IHostBuilder, IHostBuilder> select)
		: this(create, select, RunInitializedProgram.Default) {}

	protected Program(Func<IHostBuilder, IHostBuilder> select, IAllocated<IHost> run)
		: this(Host.CreateDefaultBuilder, select, run) {}

	protected Program(Func<string[], IHostBuilder> create, Func<IHostBuilder, IHostBuilder> select,
	                  IAllocated<IHost> run) : base(select, run)
	{
		_create = create;
		_select = select;
	}

	public Task Get(Array<string> arguments) => Get(_create(arguments));

	protected IHostBuilder Create(string[] parameter) => _select(_create(parameter));
}
