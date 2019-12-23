using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Activation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Services
{
	public interface IApplicationConfiguration : ICommand<IApplicationBuilder> {}

	public interface IConfigurator : IActivateUsing<IConfiguration>
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		void ConfigureServices(IServiceCollection services);

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		void Configure(IApplicationBuilder builder);
	}

	public interface IServiceConfiguration : ICommand<ConfigureParameter> {}

	public static class Extensions
	{
		public static ICommand<ConfigureParameter> Adapt(this ICommand<IServiceCollection> @this)
			=> new RegistrationConfiguration(@this.Execute);

		public static RegistrationContext<T> For<T>(this IServiceCollection @this) where T : class
			=> new RegistrationContext<T>(@this);
	}

	public sealed class RegistrationContext<T> where T : class
	{
		readonly IServiceCollection _collection;

		public RegistrationContext(IServiceCollection collection) => _collection = collection;

		public IServiceCollection Singleton<TResult>() where TResult : class, IResult<T>
			=> _collection.AddSingleton<TResult>()
			              .AddSingleton(x => x.GetRequiredService<TResult>().ToDelegate())
			              .AddSingleton(x => x.GetRequiredService<TResult>().Get());
	}

	sealed class EmptyServiceConfiguration : IServiceConfiguration
	{
		public static EmptyServiceConfiguration Default { get; } = new EmptyServiceConfiguration();

		EmptyServiceConfiguration() {}

		public void Execute(ConfigureParameter parameter) {}
	}

	public class RegistrationConfiguration : IServiceConfiguration
	{
		readonly Action<IServiceCollection> _command;

		public RegistrationConfiguration(Action<IServiceCollection> command) => _command = command;

		public void Execute(ConfigureParameter parameter)
		{
			_command(parameter.Services);
		}
	}

	/*public sealed class DefaultServiceConfiguration : ServiceConfiguration
	{
		public static DefaultServiceConfiguration Default { get; } = new DefaultServiceConfiguration();

		DefaultServiceConfiguration() : base(EmptyServiceConfiguration.Default) {}
	}*/

	public class ServiceConfiguration : Command<ConfigureParameter>, IServiceConfiguration
	{
		public ServiceConfiguration(Action<ConfigureParameter> @default)
			: this(Start.A.Command<ConfigureParameter>().By.Calling(@default)) {}

		public ServiceConfiguration(ICommand<ConfigureParameter> @default)
			: base(Start.A.Result.Of.Type<IServiceConfiguration>()
			            .By.Location.Or.Default(EmptyServiceConfiguration.Default)
			            .Assume()
			            .Then(@default)) {}
	}

	public readonly struct ConfigureParameter
	{
		public ConfigureParameter(IConfiguration configuration, IServiceCollection services)
		{
			Configuration = configuration;
			Services      = services;
		}

		public IConfiguration Configuration { get; }

		public IServiceCollection Services { get; }
	}

	public class Configurator : IConfigurator
	{
		readonly IConfiguration              _configuration;
		readonly Action<ConfigureParameter>  _services;
		readonly Action<IApplicationBuilder> _application;

		public Configurator(IConfiguration configuration, Action<ConfigureParameter> services,
		                    Action<IApplicationBuilder> application)
		{
			_configuration = configuration;
			_services      = services;
			_application   = application;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			_services(new ConfigureParameter(_configuration, services));
		}

		public void Configure(IApplicationBuilder builder)
		{
			_application(builder);
		}
	}

	sealed class EmptyApplicationConfiguration : IApplicationConfiguration
	{
		public static EmptyApplicationConfiguration Default { get; } = new EmptyApplicationConfiguration();

		EmptyApplicationConfiguration() {}

		public void Execute(IApplicationBuilder parameter) {}
	}

	public sealed class DefaultApplicationConfiguration : ApplicationConfiguration
	{
		public static DefaultApplicationConfiguration Default { get; } = new DefaultApplicationConfiguration();

		DefaultApplicationConfiguration() : base(EmptyApplicationConfiguration.Default) {}
	}

	public class ApplicationConfiguration : Command<IApplicationBuilder>, IApplicationConfiguration
	{
		public ApplicationConfiguration(ICommand<IApplicationBuilder> @default)
			: base(Start.A.Result.Of.Type<IApplicationConfiguration>()
			            .By.Location.Or.Default(EmptyApplicationConfiguration.Default)
			            .Assume()
			            .Then(@default)) {}
	}

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

	public static class RegisterOption
	{
		public static IAlteration<ConfigureParameter> Of<T>() where T : class, new() => RegisterOption<T>.Default;
	}

	sealed class RegisterOption<T> : IAlteration<ConfigureParameter> where T : class, new()
	{
		public static RegisterOption<T> Default { get; } = new RegisterOption<T>();

		RegisterOption() : this(A.Type<T>().Name) {}

		readonly string _name;

		public RegisterOption(string name) => _name = name;

		public ConfigureParameter Get(ConfigureParameter parameter)
			=> parameter.Services.Configure<T>(parameter.Configuration.GetSection(_name))
			            .AddSingleton(x => x.GetRequiredService<IOptions<T>>().Value)
			            .Return(parameter);
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