using System;
using DragonSpark.Model.Selection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;

namespace DragonSpark.Application.Mobile.Uno.Presentation.Markup;

public abstract class MarkupExtension<T> : MarkupExtension, ISelect<MarkupExtensionContext<T>, object> where T : notnull
{
	readonly ISelect<IXamlServiceProvider, IServiceProvider> _providers;

	protected MarkupExtension() : this(MarkupServiceProviders.Default) {}

	protected MarkupExtension(ISelect<IXamlServiceProvider, IServiceProvider> providers) => _providers = providers;

	protected override object ProvideValue(IXamlServiceProvider serviceProvider)
		=> Get(new(_providers.Get(serviceProvider)));

	public abstract object Get(MarkupExtensionContext<T> parameter);
}
