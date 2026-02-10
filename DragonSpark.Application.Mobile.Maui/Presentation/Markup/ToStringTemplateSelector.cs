using DragonSpark.Compose;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Markup;

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