using DragonSpark.Compose;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Runtime.Environment;
using DragonSpark.Services;
using DragonSpark.Services.Application;
using DragonSpark.Services.Compose;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Hosting.Server.Blazor
{
	public sealed class BlazorApplicationAttribute : HostingAttribute
	{
		public BlazorApplicationAttribute() : base(A.Type<BlazorApplicationAttribute>().Assembly) {}
	}

	public static class Extensions
	{
		public static ServerProfileContext WithBlazorServerApplication(this BuildHostContext @this)
			=> @this.Apply(BlazorServerProfile.Default);
	}

	sealed class DefaultApplicationConfiguration : ICommand<IApplicationBuilder>
	{
		public static DefaultApplicationConfiguration Default { get; } = new DefaultApplicationConfiguration();

		DefaultApplicationConfiguration() : this("/Error", EndpointConfiguration.Default.Execute) {}

		readonly string                        _handler;
		readonly Action<IEndpointRouteBuilder> _endpoints;

		public DefaultApplicationConfiguration(string handler, Action<IEndpointRouteBuilder> endpoints)
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

	sealed class BlazorServerProfile : ServerProfile
	{
		public static BlazorServerProfile Default { get; } = new BlazorServerProfile();

		BlazorServerProfile() : base(ServerApplicationProfile.Default.Then(DefaultServiceConfiguration.Default),
		                             DefaultApplicationConfiguration.Default.Execute) {}
	}

	sealed class DefaultServiceConfiguration : ICommand<IServiceCollection>
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