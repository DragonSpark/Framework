using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Runtime.Environment;
using DragonSpark.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Hosting.Server
{
	public sealed class ServerApplicationAttribute : HostingAttribute
	{
		public ServerApplicationAttribute() : base(typeof(ServerApplicationAttribute).Assembly) {}
	}

	sealed class DefaultEnvironmentalConfiguration : IEnvironmentalConfiguration
	{
		public static DefaultEnvironmentalConfiguration Default { get; } = new DefaultEnvironmentalConfiguration();

		DefaultEnvironmentalConfiguration() {}

		public void Execute((IApplicationBuilder Builder, IWebHostEnvironment Environment) parameter) {}
	}

	sealed class EnvironmentalConfiguration : Command<(IApplicationBuilder Builder, IWebHostEnvironment Environment)>,
	                                          IEnvironmentalConfiguration
	{
		public static EnvironmentalConfiguration Default { get; } = new EnvironmentalConfiguration();

		EnvironmentalConfiguration() : base(Start.A.Result.Of.Type<IEnvironmentalConfiguration>()
		                                         .By.Location.Or.Default(DefaultEnvironmentalConfiguration.Default)
		                                         .Assume()) {}
	}

	public sealed class ApplicationConfiguration : ICommand<IApplicationBuilder>
	{
		public static ApplicationConfiguration Default { get; } = new ApplicationConfiguration();

		ApplicationConfiguration() : this(EndpointConfiguration.Default.Execute) {}

		readonly Action<IEndpointRouteBuilder> _endpoints;

		public ApplicationConfiguration(Action<IEndpointRouteBuilder> endpoints) => _endpoints = endpoints;

		public void Execute(IApplicationBuilder parameter)
		{
			parameter.UseHttpsRedirection()
			         .UseRouting()
			         .UseAuthorization()
			         .UseEndpoints(_endpoints);
		}
	}

	public sealed class DefaultServiceConfiguration : ICommand<IServiceCollection>
	{
		public static DefaultServiceConfiguration Default { get; } = new DefaultServiceConfiguration();

		DefaultServiceConfiguration() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.AddControllers();
		}
	}

	sealed class EndpointConfiguration : ICommand<IEndpointRouteBuilder>
	{
		public static EndpointConfiguration Default { get; } = new EndpointConfiguration();

		EndpointConfiguration() {}

		public void Execute(IEndpointRouteBuilder parameter)
		{
			parameter.MapControllers();
		}
	}



	public interface IConfigurator
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		void ConfigureServices(IServiceCollection services);

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		void Configure(IApplicationBuilder app, IWebHostEnvironment env);
	}

	public class Configurator : IConfigurator
	{
		readonly Action<IServiceCollection>                                             _services;
		readonly Action<(IApplicationBuilder Builder, IWebHostEnvironment Environment)> _application;

		public Configurator(Action<IServiceCollection> services)
			: this(services, EnvironmentalConfiguration.Default.Then(ApplicationConfiguration.Default).Selector()) {}

		public Configurator(Action<IServiceCollection> services,
		                    Action<(IApplicationBuilder Builder, IWebHostEnvironment Environment)> application)
		{
			_services    = services;
			_application = application;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			_services(services);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			_application((app, env));
		}
	}
}