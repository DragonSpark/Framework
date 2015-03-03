using DragonSpark.Extensions;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interactivity;
using DragonSpark.Application.Client.Threading;
using PostSharp.Patterns.Threading;

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
			AssociatedObject.Closed += OnClosed;
			AssociatedObject.Closing += AssociatedObjectOnClosing;
		}

		[Background]
		void AssociatedObjectOnClosing( object sender, CancelEventArgs cancelEventArgs )
		{
			cancelEventArgs.Cancel.IsFalse( () => AssociatedObject.Deactivated -= OnDeactivated );
		}

		void OnLaunching( object sender, EventArgs e )
		{
			Application.SendStart();
		}

		void OnActivated( object sender, EventArgs e )
		{
			Application.SendResume();
		}

		void OnDeactivated( object sender, EventArgs e )
		{
			Application.SendSleepAsync().Wait();
		}

		void OnClosed( object sender, EventArgs e )
		{
			Application.SendSleepAsync().Wait();
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
			AssociatedObject.Closed -= OnClosed;
		}
	}
}