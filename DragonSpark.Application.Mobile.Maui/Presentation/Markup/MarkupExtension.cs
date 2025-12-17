using System;
using DragonSpark.Compose;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Markup;

public abstract class MarkupExtension<T> : IMarkupExtension<T>
{
    public abstract T ProvideValue(IServiceProvider serviceProvider);

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider)!;
}

// TODO

public sealed class ToStringTemplateSelector : DataTemplateSelector
{
    public static ToStringTemplateSelector Default { get; } = new();

    ToStringTemplateSelector() {}

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        if (container is VisualElement visual)
        {
            var key = item.ToString().Verify();
            if (visual.FindByName<DataTemplate>(key) is {} template)
            {
                return template;
            }

            if (visual.Resources.TryGetValue(key, out var resource) && resource is DataTemplate result)
            {
                return result;
            }
        }

        return null!;
    }
}