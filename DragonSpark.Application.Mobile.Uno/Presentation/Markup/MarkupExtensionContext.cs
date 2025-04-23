using System;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Uno.Presentation.Markup;

public readonly record struct MarkupExtensionContext<T>(IServiceProvider Provider, T Subject) where T : notnull
{
	public MarkupExtensionContext(IServiceProvider Provider) : this(Provider, Provider.GetRequiredService<T>()) {}
}
