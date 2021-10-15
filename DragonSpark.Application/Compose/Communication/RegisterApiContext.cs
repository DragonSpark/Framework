using DragonSpark.Compose;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Net.Http;

namespace DragonSpark.Application.Compose.Communication;

public sealed class RegisterApiContext<T> where T : class
{
	readonly IServiceCollection                     _subject;
	readonly Func<IServiceProvider, RefitSettings?> _settings;

	public RegisterApiContext(IServiceCollection subject, Func<IServiceProvider, RefitSettings?> settings)
	{
		_subject  = subject;
		_settings = settings;
	}

	public ConfiguredApiContextRegistration<T> Configure<TConfiguration>(Func<TConfiguration, Uri> baseUri)
		where TConfiguration : class
		=> Configure(new Configure<TConfiguration>(baseUri).Execute);

	public ConfiguredApiContextRegistration<T> Configure(Action<IServiceProvider, HttpClient> configure)
		=> new(_subject, configure, _settings);
}