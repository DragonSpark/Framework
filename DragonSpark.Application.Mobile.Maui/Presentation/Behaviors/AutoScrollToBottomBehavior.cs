using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using DragonSpark.Compose;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

public sealed class AutoScrollToBottomBehavior : BehaviorBase<CollectionView>
{
    protected override void OnAttached(CollectionView bindable)
    {
        base.OnAttached(bindable);
        bindable.Loaded += (_, _) =>
                           {
                               if (IsEnabled)
                               {
                                   UpdateEnabled();
                               }
                           };
    }

    static void OnIsEnabledChanged(BindableObject bindable, object _, object newValue)
    {
        bindable.To<AutoScrollToBottomBehavior>().UpdateEnabled();
    }

    void UpdateEnabled()
    {
        var view = View.Verify();
        if (IsEnabled)
        {
            view.PropertyChanged += Cv_PropertyChanged;

            if (view.ItemsSource is INotifyCollectionChanged notify)
            {
                notify.CollectionChanged += OnCollectionChanged;
            }
        }
        else
        {
            view.PropertyChanged -= Cv_PropertyChanged;

            if (view.ItemsSource is INotifyCollectionChanged notify)
            {
                notify.CollectionChanged -= OnCollectionChanged;
            }
        }
    }

    async void Cv_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is CollectionView cv && e.PropertyName == nameof(CollectionView.ItemsSource))
        {
            await ScrollToEnd(cv).Off();
        }
    }

    async void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action is NotifyCollectionChangedAction.Reset or NotifyCollectionChangedAction.Replace)
        {
            await ScrollToEnd(View.Verify()).Off();
        }
    }

    async Task ScrollToEnd(CollectionView cv)
    {
        // Use the bindable delay value (TimeSpan)
        var delay = cv.Behaviors.OfType<AutoScrollToBottomBehavior>().FirstOrDefault()?.ScrollDelay ??
                    TimeSpan.FromMilliseconds(150);
        await Task.Delay(delay).Off();

        await MainThread.InvokeOnMainThreadAsync(() =>
                                                 {
                                                     if (cv.ItemsSource is ICollection { Count: > 0 } collection)
                                                     {
                                                         var last = collection.Cast<object>().Last();
                                                         cv.ScrollTo(last, null, ScrollToPosition.End, false);

                                                         cv.InvalidateMeasure();
                                                     }
                                                 })
                        .Off();
    }

    public bool IsEnabled
    {
        get => (bool)GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }

    public readonly static BindableProperty IsEnabledProperty =
        BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(AutoScrollToBottomBehavior), true,
                                propertyChanged: OnIsEnabledChanged);

    public TimeSpan ScrollDelay
    {
        get => (TimeSpan)GetValue(ScrollDelayProperty);
        set => SetValue(ScrollDelayProperty, value);
    }

    public static readonly BindableProperty ScrollDelayProperty =
        BindableProperty.CreateAttached(nameof(ScrollDelay), typeof(TimeSpan), typeof(AutoScrollToBottomBehavior),
                                        TimeSpan.FromMilliseconds(100),
                                        validateValue: (_, value) => (TimeSpan)value >= TimeSpan.Zero);
}