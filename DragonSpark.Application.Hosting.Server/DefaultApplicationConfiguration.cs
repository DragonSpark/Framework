using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;

namespace DragonSpark.Application.Hosting.Server
{
	sealed class DefaultApplicationConfiguration : ICommand<IApplicationBuilder>
	{
		public static DefaultApplicationConfiguration Default { get; } = new DefaultApplicationConfiguration();

		DefaultApplicationConfiguration() : this(EndpointConfiguration.Default.Execute) {}

		readonly Action<IEndpointRouteBuilder> _endpoints;

		public DefaultApplicationConfiguration(Action<IEndpointRouteBuilder> endpoints) => _endpoints = endpoints;

		public void Execute(IApplicationBuilder parameter)
		{
			parameter.UseHttpsRedirection()
			         .UseRouting()
			         .UseAuthentication()
			         .UseAuthorization()
			         .UseEndpoints(_endpoints);
		}
	}
}