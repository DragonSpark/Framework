using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

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

	sealed class ServerHostBuilder<T> : ISelect<Array<string>, IHost> where T : class, IConfigurator
	{
		public static ServerHostBuilder<T> Default { get; } = new ServerHostBuilder<T>();

		ServerHostBuilder() {}

		public IHost Get(Array<string> parameter) => Host.CreateDefaultBuilder(parameter)
		                                                 .ConfigureWebHostDefaults(builder => builder.UseStartup<T>())
		                                                 .Build();
	}
}