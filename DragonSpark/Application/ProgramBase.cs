using System;
using System.Threading.Tasks;
using DragonSpark.Model.Operations.Allocated;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application;

public abstract class ProgramBase(Func<IHostBuilder, IHostBuilder> select, IAllocated<IHost> run) : IProgram
{
	protected ProgramBase(Func<IHostBuilder, IHostBuilder> select) : this(select, RunInitializedProgram.Default) {}

	public Task Get(IHostBuilder parameter) => run.Get(select(parameter).Build());
}
