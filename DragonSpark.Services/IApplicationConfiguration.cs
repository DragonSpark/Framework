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
	public interface IConfigurator : Composition.IConfigurator
	{
		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		void Configure(IApplicationBuilder builder);
	}

	public interface IApplicationConfiguration : ICommand<IApplicationBuilder> {}

	public class Configurator : Composition.Configurator
	{
		readonly Action<IApplicationBuilder> _application;

		public Configurator(ICommand<IServiceCollection> configure, ICommand<IApplicationBuilder> application)
			: this(configure.Execute, application.Execute) {}

		public Configurator(Action<IServiceCollection> configure, Action<IApplicationBuilder> application)
			: base(configure) => _application = application;

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
			: base(/*Start.A.Result.Of.Type<IApplicationConfiguration>()
			            .By.Location.Or.Default(EmptyApplicationConfiguration.Default)
			            .Assume()
			            .Then(@default)*/@default) {}
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

		public LocatedProgram(IProgram @default) : base(/*Start.A.Result.Of.Type<IProgram>()
		                                                     .By.Location.Or.Default(@default)
		                                                     .Assume()*/@default) {}
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