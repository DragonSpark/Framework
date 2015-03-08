using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class DefaultFocusElement : Behavior<Control>
	{
		public TimeSpan Delay
		{
			get { return GetValue( DelayProperty ).To<TimeSpan>(); }
			set { SetValue( DelayProperty, value ); }
		}	public static readonly DependencyProperty DelayProperty = DependencyProperty.Register( "Delay", typeof(TimeSpan), typeof(DefaultFocusElement), null );

		protected override void OnAttached()
		{
			AssociatedObject.Loaded += AssociatedObjectLoaded;
		}

		protected override void OnDetaching()
		{
			AssociatedObject.Loaded -= AssociatedObjectLoaded;
		}

		void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
		{
			Threading.Application.Delay( () => AssociatedObject.Focus(), Delay );
		}
	}
}