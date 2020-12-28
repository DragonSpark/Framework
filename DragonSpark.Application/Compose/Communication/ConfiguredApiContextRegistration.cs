using DragonSpark.Compose;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Net.Http;

namespace DragonSpark.Application.Compose.Communication
{
	public sealed class ConfiguredApiContextRegistration<T> where T : class
	{
		readonly IServiceCollection                   _subject;
		readonly Action<IServiceProvider, HttpClient> _configure;
		readonly RefitSettings?                       _settings;

		public ConfiguredApiContextRegistration(IServiceCollection subject,
		                                        Action<IServiceProvider, HttpClient> configure,
		                                        RefitSettings? settings = null)
		{
			_subject   = subject;
			_configure = configure;
			_settings  = settings;
		}

		public ConfiguredApiContextRegistration<T> Append(Action<IServiceProvider, HttpClient> next)
			=> new ConfiguredApiContextRegistration<T>(_subject,
			                                           Start.A.Command(_configure)
			                                                .Append(Start.A.Command(next))
			                                                .Get()
			                                                .Execute,
			                                           _settings);

		public IServiceCollection Then => new ConfigureApi<T>(_configure, _settings).Parameter(_subject);
	}
}