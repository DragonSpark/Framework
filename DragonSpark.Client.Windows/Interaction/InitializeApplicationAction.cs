using DragonSpark.Extensions;
using System;
using System.Windows;
using System.Windows.Interactivity;

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

	/*class InitializeApplicationAction : TriggerAction<FrameworkElement>
	{
		protected override void Invoke( object parameter )
		{
			// var host = new ApplicationHost( Application );

		}
		public global::Xamarin.Forms.Application Application
		{
			get { return GetValue( ApplicationProperty ).To<global::Xamarin.Forms.Application>(); }
			set { SetValue( ApplicationProperty, value ); }
		}	public static readonly DependencyProperty ApplicationProperty = DependencyProperty.Register( "Application", typeof(global::Xamarin.Forms.Application), typeof(InitializeApplicationAction), null );
	}*/
}
