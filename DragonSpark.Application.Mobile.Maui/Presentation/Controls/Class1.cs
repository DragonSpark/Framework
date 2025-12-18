namespace DragonSpark.Application.Mobile.Maui.Presentation.Controls;

using Microsoft.Maui.Controls;

public sealed class TemplatedContentView : ContentView
{
    public static readonly BindableProperty ItemProperty =
        BindableProperty.Create(nameof(Item), typeof(object), typeof(TemplatedContentView),
                                propertyChanged: (b, _, _) => ((TemplatedContentView)b).UpdateContent());

    public static readonly BindableProperty ItemTemplateSelectorProperty =
        BindableProperty.Create(nameof(ItemTemplateSelector), typeof(DataTemplateSelector),
                                typeof(TemplatedContentView),
                                propertyChanged: (b, _, _) => ((TemplatedContentView)b).UpdateContent());

    public TemplatedContentView() => Loaded += (_, _) => UpdateContent();

    public object? Item
    {
        get => GetValue(ItemProperty);
        set => SetValue(ItemProperty, value);
    }

    public DataTemplateSelector ItemTemplateSelector
    {
        get => (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty);
        set => SetValue(ItemTemplateSelectorProperty, value);
    }
    
    void UpdateContent()
    {
        Content = Item is not null ? ItemTemplateSelector.SelectTemplate(Item, this)?.CreateContent() as View : null;
    }
}