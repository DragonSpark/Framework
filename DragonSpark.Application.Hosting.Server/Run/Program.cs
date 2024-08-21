using DragonSpark.Model.Operations.Allocated;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Application.Hosting.Server.Run;

public class Program : DragonSpark.Application.Program
{
	protected Program(Func<IHostBuilder, IHostBuilder> select) : base(x => new ApplicationBuilder(x).Builder, select) {}

	protected Program(Func<IHostBuilder, IHostBuilder> select, IAllocated<IHost> run)
		: base(x => new ApplicationBuilder(x).Builder, select, run) {}
}