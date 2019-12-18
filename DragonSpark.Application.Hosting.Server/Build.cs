using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Environment;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
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

		public ProgramContext Environment() => Activating<LocatedProgram>();

		public ProgramContext Environment(IProgram @default) => Using(new LocatedProgram(@default));

		public ProgramContext Using(IProgram program) => new ProgramContext(program);

		public ProgramContext Activating<T>() where T : class, IProgram => Using(Start.An.Instance<T>());
	}

	public sealed class ProgramContext
	{
		readonly IProgram _program;

		public ProgramContext(IProgram program) => _program = program;

		public ISelect<Array<string>, Task> ConfiguredBy<T>() where T : class, IConfigurator
			=> HostBuilder<T>.Default.Select(_program);
	}

	public class ActivatedProgram<T> : IProgram where T : ISelect<IHost, Task>
	{
		protected ActivatedProgram() {}

		public Task Get(IHost parameter) => parameter.Services.GetRequiredService<T>().Get(parameter);
	}

	public sealed class RegisterOption<T> : IAlteration<ConfigureParameter> where T : class, new()
	{
		public static RegisterOption<T> Default { get; } = new RegisterOption<T>();

		RegisterOption() : this(A.Type<T>().Name) {}

		readonly string _name;

		public RegisterOption(string name) => _name = name;

		public ConfigureParameter Get(ConfigureParameter parameter)
			=> parameter.Services.Configure<T>(parameter.Configuration.GetSection(_name))
			            .AddSingleton(resolver => resolver.GetRequiredService<IOptions<T>>().Value)
			            .ThenWith(parameter);
	}

	public interface IProgram : ISelect<IHost, Task> {}

	public class Program : Select<IHost, Task>, IProgram
	{
		public Program(ISelect<IHost, Task> select) : base(select) {}

		public Program(Func<IHost, Task> select) : base(select) {}
	}

	sealed class LocatedProgram : Program
	{
		public static LocatedProgram Default { get; } = new LocatedProgram();

		LocatedProgram() : this(DefaultProgram.Default) {}

		public LocatedProgram(IProgram @default) : base(Start.A.Result.Of.Type<IProgram>()
		                                                     .By.Location.Or.Default(@default)
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

		public IHost Get(Array<string> parameter) => Host.CreateDefaultBuilder(parameter)
		                                                 .ConfigureWebHostDefaults(builder => builder.UseStartup<T>())
		                                                 .Build();
	}
}