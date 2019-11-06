using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Services
{
	/*public static class ApplicationConfiguration
	{
		public static Action<(IApplicationBuilder Builder, IWebHostEnvironment Environment)> For<T>()
			where T : IComponent
			=> ApplicationConfiguration<T>.Default.Execute;
	}*/

	public sealed class ApplicationConfiguration : ICommand<(IApplicationBuilder Builder, IWebHostEnvironment Environment)>
	{
		public static ApplicationConfiguration Default { get; } = new ApplicationConfiguration();

		ApplicationConfiguration() : this(EndpointConfiguration.Default.Execute) {}

		readonly Action<IEndpointRouteBuilder> _endpoints;
		readonly string                        _handler;

		public ApplicationConfiguration(Action<IEndpointRouteBuilder> endpoints, string handler = "/Home/Error")
		{
			_endpoints = endpoints;
			_handler   = handler;
		}

		public void Execute((IApplicationBuilder Builder, IWebHostEnvironment Environment) parameter)
		{
			var (builder, environment) = parameter;
			var handle = environment.IsDevelopment()
				             ? builder.UseDeveloperExceptionPage()
				             : builder.UseExceptionHandler(_handler)
				                      .UseHsts(); // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

			handle.UseHttpsRedirection()
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