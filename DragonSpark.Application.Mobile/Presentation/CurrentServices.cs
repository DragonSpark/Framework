using System;
using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Presentation;

sealed class CurrentServices : Result<IServiceProvider>, IServiceProvider
{
	public static CurrentServices Default { get; } = new();

	CurrentServices() : base(() => Microsoft.UI.Xaml.Application.Current.To<IApplication>().Host.Services) {}

	public object? GetService(Type serviceType) => Get().GetService(serviceType);
}
