using DragonSpark.Model.Selection;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;

namespace DragonSpark.Application.Communication.Http;

sealed class AddRefitClient<T, TOptions> : ISelect<AddRefitClientInput<TOptions>, IServiceCollection>
	where TOptions : Options where T : class
{
	public static AddRefitClient<T, TOptions> Default { get; } = new();

	AddRefitClient() : this(AddHttpClient<TOptions>.Default) {}

	readonly IAddClient<TOptions> _previous;

	public AddRefitClient(IAddClient<TOptions> previous) => _previous = previous;

	public IServiceCollection Get(AddRefitClientInput<TOptions> parameter)
	{
		var (subject, name, options, settings, configure) = parameter;
		return _previous.Get(new(subject, options, new AddRefitClient<T>(name, settings).Get, configure));
	}
}

sealed class AddRefitClient<T> : ISelect<IServiceCollection, IHttpClientBuilder> where T : class
{
	readonly string                                _name;
	readonly Func<IServiceProvider, RefitSettings> _settings;

	public AddRefitClient(string name, Action<IServiceProvider, RefitSettings> settings)
		: this(name, new ComposeSettings(settings).Get) {}

	public AddRefitClient(string name, Func<IServiceProvider, RefitSettings> settings)
	{
		_name     = name;
		_settings = settings;
	}

	public IHttpClientBuilder Get(IServiceCollection parameter) => parameter.AddRefitClient<T>(_settings, _name);
}