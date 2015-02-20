using DragonSpark.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using DragonSpark.Client.Windows.Presentation;
using DragonSpark.Common.Application;
using DragonSpark.ComponentModel;
using Microsoft.Practices.Prism.PubSubEvents;

namespace DragonSpark.Client.Windows.Interaction
{
	public class ActualSizeBehavior : Behavior<FrameworkElement>
	{
		protected override void OnAttached()
		{
			AssociatedObject.SizeChanged += AssociatedObjectSizeChanged;
			base.OnAttached();
			Update();
		}

		protected override void OnDetaching()
		{
			AssociatedObject.SizeChanged -= AssociatedObjectSizeChanged;
			base.OnDetaching();
		}

		public Size ActualSize
		{
			get { return GetValue( ActualSizeProperty ).To<Size>(); }
			set { SetValue( ActualSizeProperty, value ); }
		}	public static readonly DependencyProperty ActualSizeProperty = DependencyProperty.Register( "ActualSize", typeof(Size), typeof(ActualSizeBehavior), new PropertyMetadata( new Size( 0, 0 ) ) );


		void AssociatedObjectSizeChanged(object sender, SizeChangedEventArgs e)
		{
			Dispatcher.BeginInvoke( new Action( Update ) );
		}

		void Update()
		{
			ActualSize = new Size( AssociatedObject.ActualWidth, AssociatedObject.ActualHeight );
		}
	}

	public class ContentMonitorBehavior : Behavior<ContentControl>
	{
		protected override void OnAttached()
		{
			base.OnAttached();
		}

		public object Content
		{
			get { return GetValue( ContentProperty ).To<object>(); }
			set { SetValue( ContentProperty, value ); }
		}	public static readonly DependencyProperty ContentProperty = DependencyProperty.Register( "Content", typeof(object), typeof(ContentMonitorBehavior), new PropertyMetadata( ( o, args ) => o.As<ContentMonitorBehavior>( x => OnContentChanged( args.NewValue ) ) ) );

		static void OnContentChanged( object newValue )
		{
			newValue.As<FrameworkElement>( element =>
			{
				if ( element.ReadLocalValue( FrameworkElement.DataContextProperty ) == DependencyProperty.UnsetValue )
				{
					element.DataContext = element;
				}
			} );			
		}
	}
}
