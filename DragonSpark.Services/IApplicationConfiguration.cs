using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Services
{
	public static class Extensions
	{
	}

	public interface IConfigurator
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		void ConfigureServices(IServiceCollection services);

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		void Configure(IApplicationBuilder builder);
	}

	public interface IApplicationConfiguration : ICommand<IApplicationBuilder> {}

	public interface IServiceConfiguration : ICommand<IServiceCollection> {}

	sealed class EmptyServiceConfiguration : IServiceConfiguration
	{
		public static EmptyServiceConfiguration Default { get; } = new EmptyServiceConfiguration();

		EmptyServiceConfiguration() {}

		public void Execute(IServiceCollection parameter) {}
	}

	/*public class RegistrationConfiguration : IServiceConfiguration
	{
		readonly Action<IServiceCollection> _command;

		public RegistrationConfiguration(Action<IServiceCollection> command) => _command = command;

		public void Execute(ConfigureParameter parameter)
		{
			_command(parameter.Services);
		}
	}*/

	public class ServiceConfiguration : Command<IServiceCollection>, IServiceConfiguration
	{
		public ServiceConfiguration(ICommand<IServiceCollection> command) : base(command) {}

		public ServiceConfiguration(Action<IServiceCollection> command) : base(command) {}
	}

	public class LocatedServiceConfiguration : Command<IServiceCollection>, IServiceConfiguration
	{
		public LocatedServiceConfiguration(ICommand<IServiceCollection> @default)
			: base(Start.A.Result.Of.Type<IServiceConfiguration>()
			            .By.Location.Or.Default(EmptyServiceConfiguration.Default)
			            .Assume()
			            .Then(@default)) {}
	}

	/*public readonly struct ConfigureParameter
	{
		public ConfigureParameter(IHostEnvironment environment, IConfiguration configuration,
		                          IServiceCollection services)
		{
			Environment   = environment;
			Configuration = configuration;
			Services      = services;
		}

		public IHostEnvironment Environment { get; }
		public IConfiguration Configuration { get; }

		public IServiceCollection Services { get; }
	}*/

	public class Configurator : IConfigurator
	{
		readonly Action<IServiceCollection>  _configure;
		readonly Action<IApplicationBuilder> _application;

		public Configurator(Action<IServiceCollection> configure, Action<IApplicationBuilder> application)
		{
			_configure   = configure;
			_application = application;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			_configure(services);
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

	public class ActivatedProgram<T> : IProgram where T : ISelect<IHost, Task>
	{
		protected ActivatedProgram() {}

		public Task Get(IHost parameter) => parameter.Services.GetRequiredService<T>().Get(parameter);
	}

	public static class RegisterOption
	{
		public static IAlteration<IServiceCollection> Of<T>() where T : class, new() => RegisterOption<T>.Default;
	}

	sealed class RegisterOption<T> : IAlteration<IServiceCollection> where T : class, new()
	{
		public static RegisterOption<T> Default { get; } = new RegisterOption<T>();

		RegisterOption() : this(A.Type<T>().Name) {}

		readonly string _name;

		public RegisterOption(string name) => _name = name;

		public IServiceCollection Get(IServiceCollection parameter)
			=> parameter.Configure<T>(parameter.Configuration().GetSection(_name))
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