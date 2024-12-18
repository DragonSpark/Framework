using DragonSpark.Model.Selection.Stores;
using Microsoft.UI.Xaml;
using System;

namespace DragonSpark.Application.Mobile.Presentation;

sealed class MarkupServiceProviders : ReferenceValueStore<IXamlServiceProvider, IServiceProvider>
{
	public static MarkupServiceProviders Default { get; } = new();

	MarkupServiceProviders() : base(x => new MarkupServiceProvider(x)) {}
}