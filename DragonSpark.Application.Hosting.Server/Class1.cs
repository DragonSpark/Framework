using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Runtime.Environment;
using DragonSpark.Services;
using DragonSpark.Services.Application;
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

	sealed class DefaultServiceConfiguration : ServiceConfiguration
	{
		public static DefaultServiceConfiguration Default { get; } = new DefaultServiceConfiguration();

		DefaultServiceConfiguration() : base(x => x.AddControllers()) {}
	}

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
			         .UseAuthorization()
			         .UseEndpoints(_endpoints);
		}
	}

	sealed class EndpointConfiguration : Command<IEndpointRouteBuilder>
	{
		public static EndpointConfiguration Default { get; } = new EndpointConfiguration();

		EndpointConfiguration() : base(x => x.MapControllers()) {}
	}

	public sealed class ServerApplicationProfile : ServerProfile
	{
		public static IServerProfile Default { get; } = new ServerApplicationProfile();

		ServerApplicationProfile()
			: base(DefaultServiceConfiguration.Default.Execute, DefaultApplicationConfiguration.Default.Execute) {}
	}

	public static class Extension
	{
		public static ICommand<IApplicationBuilder> WithServerApplicationConfiguration(
			this ICommand<IApplicationBuilder> @this) => @this.Then(DefaultApplicationConfiguration.Default).Get();

		public static ICommand<IServiceCollection> WithServerApplicationConfiguration(
			this ICommand<IServiceCollection> @this) => @this.Then(DefaultServiceConfiguration.Default).Get();

		public static BuildHostContext WithServerApplication(this BuildHostContext @this)
			=> @this.WithServerApplication(x => x);

		public static BuildHostContext WithServerApplication(this BuildHostContext @this,
		                                                     Func<IServerProfile, IServerProfile> select)
			=> @this.Apply(select(ServerApplicationProfile.Default));

		/*

		public static BuildHostContext WithServerApplication(this BuildHostContext @this,
		                                                     ICommand<IApplicationBuilder> configure)
			=> @this.WithServerApplication(configure.Execute);

		public static BuildHostContext WithServerApplication(this BuildHostContext @this,
		                                                     Action<IApplicationBuilder> application)
			=> @this.WithServerApplication(DefaultServiceConfiguration.Default.Execute, application);

		public static BuildHostContext WithServerApplication(this BuildHostContext @this,
		                                                     Action<IServiceCollection> services,
		                                                     Action<IApplicationBuilder> application)
			=> @this.Configure(services).WithServer(application);*/
	}
}