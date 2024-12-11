using System;
using DragonSpark.Model.Operations.Allocated;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.Hosting.Uno.Run;

public abstract class Program(Func<IHostBuilder, IHostBuilder> select, IAllocated<IHost> run) : ProgramBase(select, run)
{
	protected Program(Func<IHostBuilder, IHostBuilder> select) : this(select, RunInitializedProgram.Default) {}
}
