﻿using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Runtime.Environment;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

namespace DragonSpark.Services.Application
{
	public interface IServerProfile : IServiceConfiguration, IApplicationConfiguration {}

	public class ServerProfile : IServerProfile
	{
		readonly Action<IServiceCollection>  _services;
		readonly Action<IApplicationBuilder> _application;

		public ServerProfile(Action<IServiceCollection> services, Action<IApplicationBuilder> application)
		{
			_services    = services;
			_application = application;
		}

		public void Execute(IServiceCollection parameter)
		{
			_services(parameter);
		}

		public void Execute(IApplicationBuilder parameter)
		{
			_application(parameter);
		}
	}

	sealed class ServerConfiguration : ICommand<IWebHostBuilder>
	{
		readonly Action<IApplicationBuilder> _configure;

		public ServerConfiguration(ICommand<IApplicationBuilder> configure) : this(configure.Execute) {}

		public ServerConfiguration(Action<IApplicationBuilder> configure) => _configure = configure;

		public void Execute(IWebHostBuilder parameter)
		{
			parameter.Configure(_configure);
		}
	}

	sealed class ApplyStartupConfiguration<T> : Command<IWebHostBuilder> where T : class, IStartupMarker
	{
		public static ApplyStartupConfiguration<T> Default { get; } = new ApplyStartupConfiguration<T>();

		ApplyStartupConfiguration() : base(new ApplyStartupConfiguration(A.Type<T>().Assembly.Start())) {}
	}

	sealed class ApplyStartupConfiguration : ICommand<IWebHostBuilder>
	{
		public static ApplyStartupConfiguration Default { get; } = new ApplyStartupConfiguration();

		ApplyStartupConfiguration() : this(PrimaryAssembly.Default) {}

		readonly Func<string> _name;

		public ApplyStartupConfiguration(IResult<Assembly> assembly)
			: this(assembly.Select(x => x.GetName().Name).Get) {}

		public ApplyStartupConfiguration(Func<string> name) => _name = name;

		public void Execute(IWebHostBuilder parameter)
		{
			parameter.UseStartup(_name());
		}
	}

	sealed class WebHostConfiguration : IAlteration<IHostBuilder>
	{
		readonly Action<IWebHostBuilder> _configure;

		public WebHostConfiguration(Action<IWebHostBuilder> configure) => _configure = configure;

		public IHostBuilder Get(IHostBuilder parameter) => parameter.ConfigureWebHost(_configure);
	}
}