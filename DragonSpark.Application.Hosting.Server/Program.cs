using System;
using DragonSpark.Application.Hosting.Server.Run;
using DragonSpark.Model.Operations.Allocated;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.Hosting.Server;

public class Program : DragonSpark.Application.Program
{
	protected Program(Func<IHostBuilder, IHostBuilder> select)
		: base(x => InitializeBuilder.Default.Get(x).Builder, select) {}

	protected Program(Func<IHostBuilder, IHostBuilder> select, IAllocated<IHost> run)
		: base(x => InitializeBuilder.Default.Get(x).Builder, select, run) {}
}