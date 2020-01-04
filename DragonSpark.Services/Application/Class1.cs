﻿using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Runtime.Environment;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Services.Application
{
	/*sealed class EnvironmentalStartupConfiguration : Command<IWebHostBuilder>
	{
		public static EnvironmentalStartupConfiguration Default { get; } = new EnvironmentalStartupConfiguration();

		EnvironmentalStartupConfiguration() : base(StartupConfiguration<EnvironmentalStartup>.Default) {}
	}*/

	/*sealed class StartupConfiguration<T> : ICommand<IWebHostBuilder> where T : class, IConfigurator
	{
		public static StartupConfiguration<T> Default { get; } = new StartupConfiguration<T>();

		StartupConfiguration() {}

		public void Execute(IWebHostBuilder parameter)
		{
			parameter.UseStartup<T>();
		}
	}*/

	/*sealed class EnvironmentalStartup : Configurator
	{
		public static EnvironmentalStartup Default { get; } = new EnvironmentalStartup();

		EnvironmentalStartup() : base(Composition.Compose.ConfigureFromEnvironment.Default.Execute,
		                              ConfigureFromEnvironment.Default.Execute) {}
	}*/

	public interface IServerProfile : IServiceConfiguration, IApplicationConfiguration {}

	public class ServerProfile : IServerProfile
	{
		readonly Action<IServiceCollection>  _services;
		readonly Action<IApplicationBuilder> _application;

		public ServerProfile(IServerProfile profile) : this(profile.Execute, profile.Execute) {}

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

		public ServerConfiguration(Action<IApplicationBuilder> configure) => _configure = configure;

		public void Execute(IWebHostBuilder parameter)
		{
			parameter.Configure(_configure);
		}
	}

	sealed class ApplyStartupConfiguration : ICommand<IWebHostBuilder>
	{
		public static ApplyStartupConfiguration Default { get; } = new ApplyStartupConfiguration();

		ApplyStartupConfiguration() : this(PrimaryAssembly.Default.Select(x => x.GetName().Name).Get) {}

		readonly Func<string> _name;

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