using System;
using DragonSpark.Compose;
using Microsoft.UI.Xaml;

namespace DragonSpark.Application.Mobile.Presentation;

sealed class MarkupServiceProvider : IServiceProvider
{
	readonly IXamlServiceProvider _previous;
	readonly IServiceProvider     _services;
	readonly Type                 _type;

	public MarkupServiceProvider(IXamlServiceProvider previous)
		: this(previous, Microsoft.UI.Xaml.Application.Current.To<IApplication>().Host.Services,
		       A.Type<IXamlServiceProvider>()) {}

	public MarkupServiceProvider(IXamlServiceProvider previous, IServiceProvider services, Type type)
	{
		_previous = previous;
		_services = services;
		_type     = type;
	}

	public object? GetService(Type serviceType)
		=> serviceType == _type ? _previous : _previous.GetService(serviceType) ?? _services.GetService(serviceType);
}
