using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Runtime.Environment;
using DragonSpark.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Hosting.Blazor.Server
{
	public sealed class BlazorApplicationAttribute : HostingAttribute
	{
		public BlazorApplicationAttribute() : base(typeof(BlazorApplicationAttribute).Assembly) {}
	}

	sealed class DefaultEnvironmentalConfiguration : IEnvironmentalConfiguration
	{
		public static DefaultEnvironmentalConfiguration Default { get; } = new DefaultEnvironmentalConfiguration();

		DefaultEnvironmentalConfiguration() : this("/Home/Error") {}

		readonly string _handler;

		public DefaultEnvironmentalConfiguration(string handler) => _handler = handler;

		public void Execute((IApplicationBuilder Builder, IWebHostEnvironment Environment) parameter)
		{
			parameter.Builder.UseExceptionHandler(_handler)
			         .UseHsts(); // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
		}
	}

	public sealed class EnvironmentalConfiguration
		: Command<(IApplicationBuilder Builder, IWebHostEnvironment Environment)>,
		  IEnvironmentalConfiguration
	{
		public static EnvironmentalConfiguration Default { get; } = new EnvironmentalConfiguration();

		EnvironmentalConfiguration() : base(Start.A.Result.Of.Type<IEnvironmentalConfiguration>()
		                                         .By.Using(DefaultEnvironmentalConfiguration.Default)
		                                         .ToComponent()
		                                         .Assume()) {}
	}

	/*public readonly struct ApplicationContext
	{
		public ApplicationContext(IApplicationBuilder builder, IWebHostEnvironment environment)
		{
			Builder     = builder;
			Environment = environment;
		}

		public IApplicationBuilder Builder { get; }

		public IWebHostEnvironment Environment { get; }
	}*/

	public sealed class ApplicationConfiguration : ICommand<IApplicationBuilder>
	{
		public static ApplicationConfiguration Default { get; } = new ApplicationConfiguration();

		ApplicationConfiguration() : this(EndpointConfiguration.Default.Execute) {}

		readonly Action<IEndpointRouteBuilder> _endpoints;

		public ApplicationConfiguration(Action<IEndpointRouteBuilder> endpoints) => _endpoints = endpoints;

		public void Execute(IApplicationBuilder parameter)
		{
			parameter.UseHttpsRedirection()
			         .UseStaticFiles()
			         .UseRouting()
			         .UseEndpoints(_endpoints);
		}
	}

	public sealed class DefaultServiceConfiguration : ICommand<IServiceCollection>
	{
		public static DefaultServiceConfiguration Default { get; } = new DefaultServiceConfiguration();

		DefaultServiceConfiguration() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.AddRazorPages()
			         .ThenWith(parameter)
			         .AddServerSideBlazor();
		}
	}

	sealed class ApplicationSelector : Instance<string>
	{
		public static ApplicationSelector Default { get; } = new ApplicationSelector();

		ApplicationSelector() : base("div#application") {}
	}

	sealed class EndpointConfiguration : ICommand<IEndpointRouteBuilder>
	{
		public static EndpointConfiguration Default { get; } = new EndpointConfiguration();

		EndpointConfiguration() : this(ApplicationSelector.Default, "/_Host") {}

		readonly string _selector, _fallback;

		public EndpointConfiguration(string selector, string fallback)
		{
			_selector = selector;
			_fallback = fallback;
		}

		public void Execute(IEndpointRouteBuilder parameter)
		{
			parameter.MapBlazorHub(_selector)
			         .ThenWith(parameter)
			         .MapFallbackToPage(_fallback);
		}
	}

	public interface IHostedApplication
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		void ConfigureServices(IServiceCollection services);

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		void Configure(IApplicationBuilder app, IWebHostEnvironment env);
	}

	public class HostedApplication : IHostedApplication
	{
		readonly Action<IServiceCollection>                                             _services;
		readonly Action<(IApplicationBuilder Builder, IWebHostEnvironment Environment)> _application;

		public HostedApplication(Action<IServiceCollection> services)
			: this(services, EnvironmentalConfiguration.Default.Execute) {}

		public HostedApplication(Action<IServiceCollection> services,
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