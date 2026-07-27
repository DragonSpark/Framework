namespace DragonSpark.Application.Mobile.Maui.Presentation;

public abstract class ContentPage<T> : ContentPage
{
    protected ContentPage(T context) => BindingContext = context;
}
public abstract class Shell<T> : Shell
{
    protected Shell(T context) => BindingContext = context;
}
