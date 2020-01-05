﻿using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Services.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Services.Compose
{
	public sealed class ServerProfileContext : IResult<BuildHostContext>,
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
			=> Get(x => new ServerProfile(A.Command<IServiceCollection>(x).Then(other), x.Execute));

		public ServerProfileContext Then(ICommand<IApplicationBuilder> other)
			=> Get(x => new ServerProfile(x.Execute, A.Command<IApplicationBuilder>(x).Then(other)));

		public ServerProfileContext WithEnvironmentalConfiguration()
			=> Get(x => new ServerProfile(A.Command<IServiceCollection>(x)
			                               .ConfigureFromEnvironment()
			                               .Execute,
			                              A.Command<IApplicationBuilder>(x)
			                               .Then(ConfigureFromEnvironment.Default)));

		public ServerProfileContext WithStartup<T>() where T : class, IStartupMarker
			=> new ServerProfileContext(_context, _profile,
			                            new Continuation(_selector, ApplyStartupConfiguration<T>.Default));

		public ServerProfileContext WithLocatedStartup()
			=> new ServerProfileContext(_context, _profile,
			                            new Continuation(_selector, ApplyStartupConfiguration.Default));

		public BuildHostContext Get() => _context.WithServer(_selector.Get(_profile).Execute)
		                                         .Configure(_profile);

		public ServerProfileContext Get(Func<IServerProfile, IServerProfile> parameter)
			=> new ServerProfileContext(_context, parameter(_profile), _selector);
	}

	public interface ISelector : ISelect<IServerProfile, ICommand<IWebHostBuilder>> {}

	sealed class Continuation : ISelector
	{
		readonly ISelector                 _selector;
		readonly ICommand<IWebHostBuilder> _other;

		public Continuation(ISelector selector, ICommand<IWebHostBuilder> other)
		{
			_selector = selector;
			_other    = other;
		}

		public ICommand<IWebHostBuilder> Get(IServerProfile parameter) => _selector.Get(parameter).Then(_other).Get();
	}

	sealed class Selector : ISelector
	{
		readonly IServerProfile _profile;

		public Selector(IServerProfile profile) => _profile = profile;

		public ICommand<IWebHostBuilder> Get(IServerProfile parameter) => new ServerConfiguration(_profile);
	}
}