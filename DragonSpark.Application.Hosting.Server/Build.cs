using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Environment;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Hosting.Server
{
	public sealed class Build
	{
		public static Build A { get; } = new Build();

		Build() {}

		public ProgramLocationContext Program { get; } = ProgramLocationContext.Instance;

		public sealed class ProgramLocationContext
		{
			public static ProgramLocationContext Instance { get; } = new ProgramLocationContext();

			ProgramLocationContext() {}

			public ProgramLocationByContext By { get; } = ProgramLocationByContext.Default;
		}
	}

	public sealed class ProgramLocationByContext
	{
		public static ProgramLocationByContext Default { get; } = new ProgramLocationByContext();

		ProgramLocationByContext() {}

		public ProgramContext Environment() => Activating<EnvironmentalProgram>();

		public ProgramContext Activating<T>() where T : class, IProgram => new ProgramContext(Start.An.Instance<T>());
	}

	public sealed class ProgramContext
	{
		readonly IProgram _program;

		public ProgramContext(IProgram program) => _program = program;

		public ISelect<Array<string>, Task> ConfiguredBy<T>() where T : class, IConfigurator
			=> HostBuilder<T>.Default.Select(_program);
	}

	public interface IProgram : ISelect<IHost, Task> {}

	public class Program : Select<IHost, Task>, IProgram
	{
		public Program(ISelect<IHost, Task> select) : base(select) {}

		public Program(Func<IHost, Task> select) : base(select) {}
	}

	sealed class EnvironmentalProgram : Program
	{
		public static EnvironmentalProgram Default { get; } = new EnvironmentalProgram();

		EnvironmentalProgram() : base(Start.A.Result.Of.Type<IProgram>()
		                                   .By.Location.Or.Default(DefaultProgram.Default)
		                                   .Assume()) {}
	}

	[Infrastructure]
	public sealed class DefaultProgram : IProgram
	{
		public static DefaultProgram Default { get; } = new DefaultProgram();

		DefaultProgram() {}

		public Task Get(IHost parameter) => parameter.RunAsync();
	}

	sealed class HostBuilder<T> : ISelect<Array<string>, IHost> where T : class, IConfigurator
	{
		public static HostBuilder<T> Default { get; } = new HostBuilder<T>();

		HostBuilder() {}

		public IHost Get(Array<string> parameter)
			=> Host.CreateDefaultBuilder(parameter)
			       .ConfigureWebHostDefaults(builder => builder.UseStartup<T>())
			       .Build();
	}
}