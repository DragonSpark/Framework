using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Runtime.Environment;
using DragonSpark.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Hosting.Server.Blazor
{
	public sealed class BlazorApplicationAttribute : HostingAttribute
	{
		public BlazorApplicationAttribute() : base(A.Type<BlazorApplicationAttribute>().Assembly) {}
	}

	public sealed class DefaultBlazorApplicationConfiguration : ICommand<IApplicationBuilder>
	{
		public static DefaultBlazorApplicationConfiguration Default { get; } = new DefaultBlazorApplicationConfiguration();

		DefaultBlazorApplicationConfiguration() : this("/Error", EndpointConfiguration.Default.Execute) {}

		readonly string                        _handler;
		readonly Action<IEndpointRouteBuilder> _endpoints;

		public DefaultBlazorApplicationConfiguration(string handler, Action<IEndpointRouteBuilder> endpoints)
		{
			_handler   = handler;
			_endpoints = endpoints;
		}

		public void Execute(IApplicationBuilder parameter)
		{
			parameter.UseHttpsRedirection()
			         .UseStaticFiles()
			         .UseRouting()
			         .UseEndpoints(_endpoints)
			         .UseExceptionHandler(_handler)
			         .UseHsts(); // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
		}
	}

	sealed class Configurator : Services.Configurator
	{
		public Configurator(IConfiguration configuration)
			: this(configuration, DefaultServiceConfiguration.Default.Promote().Execute) {}

		public Configurator(IConfiguration configuration, Action<ConfigureParameter> services)
			: base(configuration, services,
			       LocatedApplicationConfiguration.Default.Then(DefaultBlazorApplicationConfiguration.Default)) {}

		public Configurator(IConfiguration configuration, Action<ConfigureParameter> services,
		                    Action<IApplicationBuilder> application) : base(configuration, services, application) {}
	}

	public sealed class DefaultServiceConfiguration : ICommand<IServiceCollection>
	{
		public static DefaultServiceConfiguration Default { get; } = new DefaultServiceConfiguration();

		DefaultServiceConfiguration() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.AddRazorPages()
			         .Return(parameter)
			         .AddServerSideBlazor();
		}
	}

	sealed class ApplicationSelector : Instance<string>
	{
		public static ApplicationSelector Default { get; } = new ApplicationSelector();

		ApplicationSelector() : base("div#application") {}
	}

	sealed class DefaultApplicationSelector : Instance<string>
	{
		public static DefaultApplicationSelector Default { get; } = new DefaultApplicationSelector();

		DefaultApplicationSelector() : base("/_blazor") {}
	}

	sealed class EndpointConfiguration : ICommand<IEndpointRouteBuilder>
	{
		public static EndpointConfiguration Default { get; } = new EndpointConfiguration();

		EndpointConfiguration() : this(DefaultApplicationSelector.Default, "/_Host") {}

		readonly string _selector, _fallback;

		public EndpointConfiguration(string selector, string fallback)
		{
			_selector = selector;
			_fallback = fallback;
		}

		public void Execute(IEndpointRouteBuilder parameter)
		{
			parameter.MapBlazorHub(_selector)
			         .Return(parameter)
			         .MapFallbackToPage(_fallback);
		}
	}
}