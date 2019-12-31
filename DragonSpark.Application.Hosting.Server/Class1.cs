using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Runtime.Environment;
using DragonSpark.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Hosting.Server
{
	public sealed class ServerApplicationAttribute : HostingAttribute
	{
		public ServerApplicationAttribute() : base(A.Type<ServerApplicationAttribute>().Assembly) {}
	}

	public sealed class DefaultApplicationConfiguration : ApplicationConfiguration
	{
		public static DefaultApplicationConfiguration Default { get; } = new DefaultApplicationConfiguration();

		DefaultApplicationConfiguration() : base(ServerApplicationConfiguration.Default) {}
	}

	sealed class ServerApplicationConfiguration : ICommand<IApplicationBuilder>
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

	public sealed class DefaultServiceConfiguration : LocatedServiceConfiguration
	{
		public static DefaultServiceConfiguration Default { get; } = new DefaultServiceConfiguration();

		DefaultServiceConfiguration() : base(RegistrationConfiguration.Default) {}
	}

	sealed class RegistrationConfiguration : ServiceConfiguration
	{
		public static RegistrationConfiguration Default { get; } = new RegistrationConfiguration();

		RegistrationConfiguration() : base(x => x.AddControllers()) {}
	}

	sealed class EndpointConfiguration : Command<IEndpointRouteBuilder>
	{
		public static EndpointConfiguration Default { get; } = new EndpointConfiguration();

		EndpointConfiguration() : base(x => x.MapControllers()) {}
	}

	public class Configurator : Services.Configurator
	{
		public Configurator() : this(DefaultServiceConfiguration.Default.Execute) {}

		public Configurator(Action<IServiceCollection> services)
			: this(services, DefaultApplicationConfiguration.Default.Execute) {}

		public Configurator(Action<IServiceCollection> configure, Action<IApplicationBuilder> application)
			: base(configure, application) {}
	}
}