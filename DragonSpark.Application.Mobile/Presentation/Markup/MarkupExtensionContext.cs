using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Mobile.Presentation;

public readonly record struct MarkupExtensionContext<T>(IServiceProvider Provider, T Subject) where T : notnull
{
	public MarkupExtensionContext(IServiceProvider Provider) : this(Provider, Provider.GetRequiredService<T>()) {}
}