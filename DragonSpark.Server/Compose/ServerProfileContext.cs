using DragonSpark.Compose;
using DragonSpark.Compose.Model;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Environment;
using DragonSpark.Server.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace DragonSpark.Server.Compose
{
	public sealed class ServerProfileContext : ISelect<ICommand<IWebHostBuilder>, ServerProfileContext>,
	                                           ISelect<Func<IServerProfile, IServerProfile>, ServerProfileContext>
	{
		readonly BuildHostContext                _context;
		readonly IServerProfile                  _profile;
		readonly CommandContext<IWebHostBuilder> _configure;

		public ServerProfileContext(BuildHostContext context, IServerProfile profile)
			: this(context, profile, Start.A.Command<IWebHostBuilder>().By.Empty) {}

		public ServerProfileContext(BuildHostContext context, IServerProfile profile,
		                            CommandContext<IWebHostBuilder> configure)
		{
			_context   = context;
			_profile   = profile;
			_configure = configure;
		}

		public ServerProfileContext Then(ICommand<IServiceCollection> other)
			=> Get(x => new ServerProfile(A.Command<IServiceCollection>(x).Then().Append(other), x.Execute));

		public ServerProfileContext Then(ICommand<IApplicationBuilder> other)
			=> Get(x => new ServerProfile(x.Execute, A.Command<IApplicationBuilder>(x).Then().Append(other)));

		public ServerProfileContext WithEnvironmentalConfiguration()
			=> Get(x => new ServerProfile(A.Command<IServiceCollection>(x)
			                               .ConfigureFromEnvironment()
			                               .Execute,
			                              A.Command<IApplicationBuilder>(x)
			                               .Then()
			                               .Append(ConfigureFromEnvironment.Default)));

		public ServerProfileContext NamedFromPrimaryAssembly() => Named(PrimaryAssembly.Default);

		public ServerProfileContext Named(IResult<Assembly> assembly) => Get(new ApplyNameConfiguration(assembly));

		public BuildServerContext As => new BuildServerContext(_context, _profile.Execute,
		                                                       _configure.Prepend(new ServerConfiguration(_profile)));

		public ServerProfileContext Get(Func<IServerProfile, IServerProfile> parameter)
			=> new ServerProfileContext(_context, parameter(_profile), _configure);

		public ServerProfileContext Get(ICommand<IWebHostBuilder> parameter)
			=> new ServerProfileContext(_context, _profile, _configure.Append(parameter));
	}

	public sealed class BuildServerContext
	{
		readonly BuildHostContext                  _context;
		readonly System.Action<IServiceCollection> _services;
		readonly System.Action<IWebHostBuilder>    _application;

		public BuildServerContext(BuildHostContext context, System.Action<IServiceCollection> services,
		                          System.Action<IWebHostBuilder> application)
		{
			_context     = context;
			_services    = services;
			_application = application;
		}

		public BuildHostContext Application()
			=> _context.Select(new ApplicationWebHostConfiguration(_application)).Configure(_services);

		public BuildHostContext Is()
			=> _context.Select(new WebHostConfiguration(_application)).Configure(_services);
	}
}