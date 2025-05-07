using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Presentation;

public abstract class ContentPage<T> : ContentPage
{
    protected ContentPage(T context) => BindingContext = context;
}
