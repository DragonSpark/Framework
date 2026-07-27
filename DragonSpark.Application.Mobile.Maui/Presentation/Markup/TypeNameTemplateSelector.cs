using DragonSpark.Compose;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Markup;

public sealed class TypeNameTemplateSelector : DataTemplateSelector
{
    public static TypeNameTemplateSelector Default { get; } = new();

    TypeNameTemplateSelector() : this("Default") {}

    readonly string _default;

    public TypeNameTemplateSelector(string @default) => _default = @default;

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        if (container is VisualElement visual)
        {
            var key = item.Account()?.GetType().Name ?? _default;
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