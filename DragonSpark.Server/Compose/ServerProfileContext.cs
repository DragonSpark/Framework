using DragonSpark.Compose;
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
	public sealed class ServerProfileContext :// IResult<BuildHostContext>,
	                                           ISelect<ICommand<IWebHostBuilder>, ServerProfileContext>,
	                                           ISelect<Func<IServerProfile, IServerProfile>, ServerProfileContext>
	{
		readonly BuildHostContext _context;
		readonly IServerProfile   _profile;
		readonly ISelector        _selector;

		public ServerProfileContext(BuildHostContext context, IServerProfile profile)
			: this(context, profile, new Selector(profile)) {}

		public ServerProfileContext(BuildHostContext context, IServerProfile profile, ISelector selector)
		{
			_context  = context;
			_profile  = profile;
			_selector = selector;
		}

		public ServerProfileContext Then(ICommand<IServiceCollection> other)
			=> Get(x => new ServerProfile(A.Command<IServiceCollection>(x).Then().Add(other), x.Execute));

		public ServerProfileContext Then(ICommand<IApplicationBuilder> other)
			=> Get(x => new ServerProfile(x.Execute, A.Command<IApplicationBuilder>(x).Then().Add(other)));

		public ServerProfileContext WithEnvironmentalConfiguration()
			=> Get(x => new ServerProfile(A.Command<IServiceCollection>(x)
			                               .ConfigureFromEnvironment()
			                               .Execute,
			                              A.Command<IApplicationBuilder>(x)
			                               .Then()
			                               .Add(ConfigureFromEnvironment.Default)));

		public ServerProfileContext NamedFromPrimaryAssembly() => Named(PrimaryAssembly.Default);

		public ServerProfileContext Named(IResult<Assembly> assembly) => Get(new ApplyNameConfiguration(assembly));

		
		public BuildServerContext As => new BuildServerContext(_context, _profile.Execute, 
		                                                       _selector.Get(_profile).Execute);

		public ServerProfileContext Get(Func<IServerProfile, IServerProfile> parameter)
			=> new ServerProfileContext(_context, parameter(_profile), _selector);

		public ServerProfileContext Get(ICommand<IWebHostBuilder> parameter)
			=> new ServerProfileContext(_context, _profile, new Continuation(_selector, parameter));
	}

	public sealed class BuildServerContext
	{
		readonly BuildHostContext           _context;
		readonly Action<IServiceCollection> _services;
		readonly Action<IWebHostBuilder>    _application;

		public BuildServerContext(BuildHostContext context, Action<IServiceCollection> services,
		                          Action<IWebHostBuilder> application)
		{
			_context     = context;
			_services    = services;
			_application = application;
		}

		public BuildHostContext Application()
			=> _context.Select(new ApplicationWebHostConfiguration(_application)).Configure(_services);

		public BuildHostContext General()
			=> _context.Select(new WebHostConfiguration(_application)).Configure(_services);
	}

	public interface ISelector : ISelect<IServerProfile, ICommand<IWebHostBuilder>> {}

	sealed class Continuation : ISelector
	{
		readonly ISelector                 _selector;
		readonly ICommand<IWebHostBuilder> _next;

		public Continuation(ISelector selector, ICommand<IWebHostBuilder> next)
		{
			_selector = selector;
			_next     = next;
		}

		public ICommand<IWebHostBuilder> Get(IServerProfile parameter)
			=> _selector.Get(parameter).Then().Add(_next).Get();
	}

	sealed class Selector : FixedResult<IServerProfile, ICommand<IWebHostBuilder>>, ISelector
	{
		public Selector(IServerProfile profile) : base(new ServerConfiguration(profile)) {}
	}
}