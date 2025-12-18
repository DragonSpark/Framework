using System.Windows.Input;
using DragonSpark.Compose;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

public sealed class NavigatingCommandBehavior : BehaviorBase<WebView>
{
    public static readonly BindableProperty AddressProperty =
        BindableProperty.Create(nameof(Address), typeof(string), typeof(NavigatingCommandBehavior), string.Empty);

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(NavigatingCommandBehavior));

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(NavigatingCommandBehavior));

    public string Address
    {
        get => (string)GetValue(AddressProperty);
        set => SetValue(AddressProperty, value);
    }

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    protected override void OnAttachedTo(WebView webView)
    {
        base.OnAttachedTo(webView);
        webView.Navigating += OnNavigating;
    }

    protected override void OnDetachingFrom(WebView webView)
    {
        webView.Navigating -= OnNavigating;
        base.OnDetachingFrom(webView);
    }

    void OnNavigating(object? sender, WebNavigatingEventArgs e)
    {
        e.Cancel = !Address.IsNullOrEmpty() && e.Url.StartsWith(Address);
        if (e.Cancel && Command?.CanExecute(CommandParameter) == true)
        {
            MainThread.BeginInvokeOnMainThread(() => Command.Execute(CommandParameter));
        }
    }
}