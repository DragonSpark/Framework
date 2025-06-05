using System;
using Microsoft.Maui.Controls.Xaml;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Markup;

public abstract class MarkupExtension<T> : IMarkupExtension<T>
{
    public abstract T ProvideValue(IServiceProvider serviceProvider);

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}