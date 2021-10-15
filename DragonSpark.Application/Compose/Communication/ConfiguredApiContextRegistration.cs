using DragonSpark.Compose;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Net.Http;

namespace DragonSpark.Application.Compose.Communication;

public sealed class ConfiguredApiContextRegistration<T> where T : class
{
	readonly IServiceCollection                     _subject;
	readonly Action<IServiceProvider, HttpClient>   _configure;
	readonly Func<IServiceProvider, RefitSettings?> _settings;

	public ConfiguredApiContextRegistration(IServiceCollection subject,
	                                        Action<IServiceProvider, HttpClient> configure,
	                                        Func<IServiceProvider, RefitSettings?> settings)
	{
		_subject   = subject;
		_configure = configure;
		_settings  = settings;
	}

	public ConfiguredApiContextRegistration<T> Append(Action<IServiceProvider, HttpClient> next)
		=> new(_subject, Start.A.Command(_configure).Append(Start.A.Command(next)).Get().Execute, _settings);

	public IServiceCollection Then => new ConfigureApi<T>(_configure, _settings).Parameter(_subject);
}