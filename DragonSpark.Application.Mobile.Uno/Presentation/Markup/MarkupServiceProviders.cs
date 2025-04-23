using System;
using DragonSpark.Model.Selection.Stores;
using Microsoft.UI.Xaml;

namespace DragonSpark.Application.Mobile.Uno.Presentation.Markup;

sealed class MarkupServiceProviders : ReferenceValueStore<IXamlServiceProvider, IServiceProvider>
{
	public static MarkupServiceProviders Default { get; } = new();

	MarkupServiceProviders() : base(x => new MarkupServiceProvider(x)) {}
}
