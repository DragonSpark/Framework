using DragonSpark.Compose;
using DragonSpark.Model.Commands;
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

	sealed class Configurator : Services.Configurator
	{
		public Configurator(IConfiguration configuration) : this(configuration, ServiceConfiguration.Default.Execute) {}

		public Configurator(IConfiguration configuration, Action<ConfigureParameter> services)
			: this(configuration, services,
			       LocatedApplicationConfiguration.Default.Then(ServerApplicationConfiguration.Default)) {}

		public Configurator(IConfiguration configuration, Action<ConfigureParameter> services,
		                    Action<IApplicationBuilder> application) : base(configuration, services, application) {}
	}
}