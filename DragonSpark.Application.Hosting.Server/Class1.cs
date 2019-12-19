using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Runtime.Activation;
using DragonSpark.Runtime.Environment;
using DragonSpark.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Hosting.Server
{
	public sealed class ServerApplicationAttribute : HostingAttribute
	{
		public ServerApplicationAttribute() : base(A.Type<ServerApplicationAttribute>().Assembly) {}
	}

	sealed class DefaultApplicationConfiguration : IApplicationConfiguration
	{
		public static DefaultApplicationConfiguration Default { get; } = new DefaultApplicationConfiguration();

		DefaultApplicationConfiguration() {}

		public void Execute(IApplicationBuilder parameter) {}
	}

	sealed class ApplicationConfiguration : Command<IApplicationBuilder>, IApplicationConfiguration
	{
		public static ApplicationConfiguration Default { get; } = new ApplicationConfiguration();

		ApplicationConfiguration() : base(Start.A.Result.Of.Type<IApplicationConfiguration>()
		                                       .By.Location.Or.Default(DefaultApplicationConfiguration.Default)
		                                       .Assume()) {}
	}

	public sealed class ServerApplicationConfiguration : ICommand<IApplicationBuilder>
	{
		public static ServerApplicationConfiguration Default { get; } = new ServerApplicationConfiguration();

		ServerApplicationConfiguration() : this(EndpointConfiguration.Default.Execute) {}

		readonly Action<IEndpointRouteBuilder> _endpoints;

		public ServerApplicationConfiguration(Action<IEndpointRouteBuilder> endpoints) => _endpoints = endpoints;

		public void Execute(IApplicationBuilder parameter)
		{
			parameter.UseHttpsRedirection()
			         .UseRouting()
			         .UseAuthorization()
			         .UseEndpoints(_endpoints);
		}
	}

	public interface IServiceConfiguration : ICommand<ConfigureParameter> {}

	public static class ServicesConfiguration
	{
		public static ICommand<ConfigureParameter> Promote(this ICommand<IServiceCollection> @this)
			=> new RegistrationConfiguration(@this.Execute);
	}

	public static class Extensions
	{
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

	[Infrastructure]
	public class RegistrationConfiguration : IServiceConfiguration
	{
		readonly Action<IServiceCollection> _command;

		public RegistrationConfiguration(Action<IServiceCollection> command) => _command = command;

		public void Execute(ConfigureParameter parameter)
		{
			_command(parameter.Services);
		}
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

	[Infrastructure]
	public sealed class ServiceConfiguration : Command<ConfigureParameter>, IServiceConfiguration
	{
		public static ServiceConfiguration Default { get; } = new ServiceConfiguration();

		ServiceConfiguration() : base(Start.A.Result.Of.Type<IServiceConfiguration>()
		                                   .By.Location.Or.Default(DefaultServiceConfiguration.Default)
		                                   .Assume()) {}
	}

	[Infrastructure]
	public sealed class DefaultServiceConfiguration : RegistrationConfiguration
	{
		public static DefaultServiceConfiguration Default { get; } = new DefaultServiceConfiguration();

		DefaultServiceConfiguration() : base(x => x.AddControllers()) {}
	}

	sealed class EndpointConfiguration : Command<IEndpointRouteBuilder>
	{
		public static EndpointConfiguration Default { get; } = new EndpointConfiguration();

		EndpointConfiguration() : base(x => x.MapControllers()) {}
	}

	public interface IConfigurator : IActivateUsing<IConfiguration>
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		void ConfigureServices(IServiceCollection services);

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		void Configure(IApplicationBuilder builder);
	}

	public class Configurator : IConfigurator
	{
		readonly IConfiguration              _configuration;
		readonly Action<ConfigureParameter>  _services;
		readonly Action<IApplicationBuilder> _application;

		public Configurator(IConfiguration configuration, Action<ConfigureParameter> services)
			: this(configuration, services,
			       ApplicationConfiguration.Default.Then(ServerApplicationConfiguration.Default)) {}

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
}