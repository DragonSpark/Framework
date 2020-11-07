﻿using DragonSpark.Compose;
using DragonSpark.Compose.Model;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Environment;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace DragonSpark.Application.Compose
{
	public sealed class ApplicationProfileContext
		: ISelect<ICommand<IWebHostBuilder>, ApplicationProfileContext>,
		  ISelect<Func<IApplicationProfile, IApplicationProfile>, ApplicationProfileContext>
	{
		readonly CommandContext<IWebHostBuilder> _configure;
		readonly BuildHostContext                _context;
		readonly IApplicationProfile             _profile;

		public ApplicationProfileContext(BuildHostContext context, IApplicationProfile profile)
			: this(context, profile, Start.A.Command<IWebHostBuilder>().By.Empty) {}

		public ApplicationProfileContext(BuildHostContext context, IApplicationProfile profile,
		                                 CommandContext<IWebHostBuilder> configure)
		{
			_context   = context;
			_profile   = profile;
			_configure = configure;
		}

		public BuildServerContext As => new BuildServerContext(_context, _profile.Execute,
		                                                       _configure.Prepend(new ServerConfiguration(_profile)));

		public ApplicationProfileContext Configure(Func<BuildHostContext, BuildHostContext> context)
			=> new ApplicationProfileContext(context(_context), _profile, _configure);

		public ApplicationProfileContext Then(System.Action<IServiceCollection> other) => Then(Start.A.Command(other).Get());

		public ApplicationProfileContext Then(ICommand<IServiceCollection> other)
			=> Get(x => new ApplicationProfile(A.Command<IServiceCollection>(x).Then().Append(other), x.Execute));

		public ApplicationProfileContext Then(System.Action<IApplicationBuilder> other) => Then(Start.A.Command(other).Get());

		public ApplicationProfileContext Then(ICommand<IApplicationBuilder> other)
			=> Get(x => new ApplicationProfile(x.Execute, A.Command<IApplicationBuilder>(x).Then().Append(other)));

		public ApplicationProfileContext WithEnvironmentalConfiguration()
			=> Get(x => new ApplicationProfile(A.Command<IServiceCollection>(x)
			                                    .ConfigureFromEnvironment()
			                                    .Execute,
			                                   ConfigureFromEnvironment.Default.Then().Append(x)));

		public ApplicationProfileContext NamedFromPrimaryAssembly() => Named(PrimaryAssembly.Default);

		public ApplicationProfileContext Named(IResult<Assembly> assembly) => Get(new ApplyNameConfiguration(assembly));

		public ApplicationProfileContext Get(Func<IApplicationProfile, IApplicationProfile> parameter)
			=> new ApplicationProfileContext(_context, parameter(_profile), _configure);

		public ApplicationProfileContext Get(ICommand<IWebHostBuilder> parameter)
			=> new ApplicationProfileContext(_context, _profile, _configure.Append(parameter));
	}
}