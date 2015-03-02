
using DragonSpark.Application.Client.Eventing;
using DragonSpark.Application.Client.Extensions;
using DragonSpark.Extensions;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interactivity;

namespace DragonSpark.Application.Client.Forms.Interaction
{
	public class FormsEventsBehavior : Behavior<Window>
	{
		protected override void OnAttached()
		{
			base.OnAttached();
			
			AssociatedObject.Loaded += OnLaunching;
			AssociatedObject.Activated += OnActivated;
			AssociatedObject.Deactivated += OnDeactivated;
			AssociatedObject.Closed +=OnClosed;
		}

		void OnLaunching( object sender, EventArgs e )
		{
			Application.SendStart();
		}

		void OnActivated( object sender, EventArgs e )
		{
			Application.SendResume();
		}

		async void OnDeactivated( object sender, EventArgs e )
		{
			await Application.SendSleepAsync();
		}

		async void OnClosed( object sender, EventArgs e )
		{
			await Application.SendSleepAsync();
		}

		public Xamarin.Forms.Application Application
		{
			get { return GetValue( ApplicationProperty ).To<Xamarin.Forms.Application>(); }
			set { SetValue( ApplicationProperty, value ); }
		}	public static readonly DependencyProperty ApplicationProperty = DependencyProperty.Register( "Application", typeof(Xamarin.Forms.Application), typeof(FormsEventsBehavior), null );


		protected override void OnDetaching()
		{
			base.OnDetaching();

			AssociatedObject.Loaded -= OnLaunching;
			AssociatedObject.Activated -= OnActivated;
			AssociatedObject.Deactivated -= OnDeactivated;
			AssociatedObject.Closed -=OnClosed;
		}
	}

	public class CloseOnReturningBehavior : Behavior<FrameworkElement>
	{
		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.Loaded += AssociatedObjectOnLoaded;
			AssociatedObject.Unloaded += AssociatedObjectOnUnloaded;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();

			AssociatedObject.Loaded -= AssociatedObjectOnLoaded;
			AssociatedObject.Unloaded -= AssociatedObjectOnUnloaded;
		}

		void AssociatedObjectOnLoaded( object sender, RoutedEventArgs routedEventArgs )
		{
			this.Event<ReturningEvent>().Subscribe( OnReturning );
		}

		void OnReturning( CancelEventArgs eventArgs )
		{
			eventArgs.Cancel = true;
			var window = AssociatedObject.GetParentOfType<Window>();
			window.Close();
		}
		
		void AssociatedObjectOnUnloaded( object sender, RoutedEventArgs routedEventArgs )
		{
			this.Event<ReturningEvent>().Unsubscribe( OnReturning );
		}
	}
}
